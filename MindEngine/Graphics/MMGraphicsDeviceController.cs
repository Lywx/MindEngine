namespace MindEngine.Graphics
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Core.Component;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     It is a wrapper class around the GraphicsDevice.
    /// </summary>
    public class MMGraphicsDeviceController : MMCompositeComponent, IMMGraphicsDeviceController
    {
        #region Constructors and Finalizer

        public MMGraphicsDeviceController(MMEngine engine) : base(engine)
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            base.Initialize();

            this.MatrixInitialize();
        }

        #endregion

        #region Components

        public SpriteBatch SpriteBatch { get; }

        //TODO(Wuxiang)
        //public MMViewportAdapter ViewportAdapter { get; set; } = new MMDefaultViewportAdapter();

        public Viewport Viewport => this.GraphicsDevice.Viewport;

        #endregion

        #region Render Target 

        private RenderTarget2D currentRenderTarget;

        private RenderTarget2D previousRenderTarget;

        internal RenderTarget2D CurrentRenderTarget
        {
            get { return this.currentRenderTarget; }
            set
            {
                this.previousRenderTarget = this.currentRenderTarget;
                this.currentRenderTarget = value;

                if (this.GraphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.Normal)
                {
                    this.GraphicsDevice.SetRenderTarget(this.currentRenderTarget);
                }
            }
        }

        public void SetRenderTarget(RenderTarget2D target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            this.CurrentRenderTarget = target;
        }

        public void RestoreRenderTarget()
        {
            this.CurrentRenderTarget = this.previousRenderTarget;
        }

        #endregion

        #region Scissor Test

        private readonly List<RasterizerState> rasterizerStatesCache = new List<RasterizerState>();

        public Rectangle ScissorRectangle
        {
            get { return this.GraphicsDevice.ScissorRectangle; }
            set { this.GraphicsDevice.ScissorRectangle = value; }
        }

        /// <reference>
        ///     http://gamedev.stackexchange.com/questions/24697/how-to-clip-cut-off-text-in-a-textbox
        /// </reference>
        /// <summary>
        ///     Whether enable scissor in rasterization.
        /// </summary>
        /// <remarks>
        ///     Set it to true, before using the scissor. After drawing using sprite
        ///     batch, you could set it to false before sprite batch end.
        /// </remarks>
        public bool ScissorRectangleEnabled
        {
            get { return this.GraphicsDevice.RasterizerState.ScissorTestEnable; }
            set
            {
                var changed = this.GraphicsDevice.RasterizerState.ScissorTestEnable != value;
                if (changed)
                {
                    this.GraphicsDevice.RasterizerState = this.GetScissorRasterizerState(value);
                }
            }
        }

        private RasterizerState GetScissorRasterizerState(bool scissorEnabled)
        {
            var currentState = this.GraphicsDevice.RasterizerState;

            for (var i = 0; i < this.rasterizerStatesCache.Count; i++)
            {
                var state = this.rasterizerStatesCache[i];
                if (state.ScissorTestEnable == scissorEnabled
                    && currentState.CullMode == state.CullMode

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    && currentState.DepthBias == state.DepthBias
                    && currentState.FillMode == state.FillMode
                    && currentState.MultiSampleAntiAlias == state.MultiSampleAntiAlias

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    && currentState.SlopeScaleDepthBias == state.SlopeScaleDepthBias)
                {
                    return state;
                }
            }

            var newState = new RasterizerState
            {
                ScissorTestEnable = scissorEnabled,
                CullMode = currentState.CullMode,
                DepthBias = currentState.DepthBias,
                FillMode = currentState.FillMode,
                MultiSampleAntiAlias = currentState.MultiSampleAntiAlias,
                SlopeScaleDepthBias = currentState.SlopeScaleDepthBias
            };

            this.rasterizerStatesCache.Add(newState);

            return newState;
        }

        #endregion

        #region Effect Texture

        private bool vertexColorEnabled;

        public bool VertexColorEnabled
        {
            get { return this.vertexColorEnabled; }
            set
            {
                if (this.vertexColorEnabled != value)
                {
                    this.vertexColorEnabled = value;
                    this.textureChanged = true;
                }
            }
        }

        private Texture2D textureCurrent;

        private bool textureChanged;

        private bool textureEnabled;

        public bool TextureEnabled
        {
            get { return this.textureEnabled; }
            set
            {
                if (this.textureEnabled != value)
                {
                    this.textureEnabled = value;
                    this.textureChanged = true;
                }
            }
        }

        private void TextureApply()
        {
            if (this.effectCurrent is BasicEffect)
            {
                var basicEffect = (BasicEffect)this.effectCurrent;

                basicEffect.Texture = this.textureCurrent;
                basicEffect.TextureEnabled = this.textureEnabled;
                basicEffect.VertexColorEnabled = this.vertexColorEnabled;
            }
            else if (this.effectCurrent is AlphaTestEffect)
            {
                var alphaTestEffect = (AlphaTestEffect)this.effectCurrent;

                alphaTestEffect.Texture = this.textureCurrent;
                alphaTestEffect.VertexColorEnabled = this.vertexColorEnabled;
            }
            else
            {
                throw new Exception(
                    $"Effect {this.effectCurrent.GetType().Name} is not supported");
            }
        }

        public void TextureSet(Texture2D texture)
        {
            if (!this.GraphicsDevice.IsDisposed
                && this.GraphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.Normal)
            {
                if (texture == null)
                {
                    this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                    this.TextureEnabled = false;
                }
                else
                {
                    // TODO(Improvement): Not supported for texture customized sampler state.
                    // this.GraphicsDevice.SamplerStates[0] = texture.SamplerState;

                    this.TextureEnabled = true;
                }

                if (this.textureCurrent != texture)
                {
                    this.textureCurrent = texture;
                    this.textureChanged = true;
                }
            }
        }

        private void TextureReset()
        {
            this.vertexColorEnabled = true;

            this.textureChanged = false;
            this.textureCurrent = null;
        }

        #endregion

        #region Effect Operations

        private bool effectChanged;

        private Effect effectCurrent;

        private BasicEffect effectDefault;

        private readonly Stack<Effect> effectStack;

        internal void EffectPush(Effect effect)
        {
            this.effectStack.Push(this.effectCurrent);
            this.effectCurrent = effect;
            this.effectChanged = true;
        }

        internal void EffectPop()
        {
            this.effectCurrent = this.effectStack.Pop();
            this.effectChanged = true;
        }

        private void EffectApply()
        {
            if (this.effectChanged)
            {
                var matrices = this.effectCurrent as IEffectMatrices;

                if (matrices != null)
                {
                    matrices.World = this.matrixWorld;
                    matrices.View = this.matrixView;
                    matrices.Projection = this.matrixProjection;
                }

                this.TextureApply();
            }
            else
            {
                if (this.matrixWorldChanged
                    || this.matrixProjectionChanged
                    || this.matrixViewChanged)
                {
                    var matrices = this.effectCurrent as IEffectMatrices;
                    if (matrices != null)
                    {
                        if (this.matrixWorldChanged)
                        {
                            matrices.World = this.matrixWorld;
                        }

                        if (this.matrixViewChanged)
                        {
                            matrices.View = this.matrixView;
                        }

                        if (this.matrixProjectionChanged)
                        {
                            matrices.Projection = this.matrixProjection;
                        }
                    }
                }

                if (this.textureChanged)
                {
                    this.TextureApply();
                }
            }

            this.effectChanged = false;
            this.textureChanged = false;

            this.matrixWorldChanged = false;
            this.matrixViewChanged = false;
            this.matrixProjectionChanged = false;
        }

        private void EffectResetDefault()
        {
            this.effectDefault.Alpha = 1f;
            this.effectDefault.TextureEnabled = false;
            this.effectDefault.Texture = null;
            this.effectDefault.VertexColorEnabled = true;

            this.effectDefault.View = this.matrixView;
            this.effectDefault.World = this.matrixWorld;
            this.effectDefault.Projection = this.matrixProjection;
        }

        private void EffectReset()
        {
            this.effectChanged = false;

            this.effectStack.Clear();
            this.effectCurrent = this.effectDefault;
        }

        #endregion

        #region Effect Matrix

        private readonly Stack<Matrix> matrixStack = new Stack<Matrix>();

        private Matrix matrixWorld = Matrix.Identity;

        private Matrix matrixView = Matrix.Identity;

        private Matrix matrixProjection = Matrix.Identity;

        private Matrix matrixCurrent = Matrix.Identity;

        private bool matrixWorldChanged;

        private bool matrixProjectionChanged;

        private bool matrixViewChanged;

        private void MatrixInitialize()
        {
            this.matrixProjection = Matrix.Identity;
            this.matrixView = Matrix.Identity;
            this.matrixWorld = Matrix.Identity;

            this.matrixWorldChanged = true;
            this.matrixViewChanged = true;
            this.matrixProjectionChanged = true;
        }

        public void MatrixMultiply(Matrix matrixIn)
        {
            this.matrixCurrent = Matrix.Multiply(this.matrixCurrent, matrixIn);
            this.matrixWorldChanged = true;
        }

        public void MatrixPush()
        {
            this.matrixStack.Push(this.matrixCurrent);
        }

        public void MatrixPop()
        {
            this.matrixCurrent = this.matrixStack.Pop();
            this.matrixWorldChanged = true;
        }

        public void MatrixSetIdentity()
        {
            this.matrixCurrent = Matrix.Identity;
            this.matrixWorldChanged = true;
        }

        private void MatrixReset()
        {
            this.matrixWorldChanged = false;
            this.matrixViewChanged = false;
            this.matrixProjectionChanged = false;
        }

        #endregion

        #region Reset

        internal void Reset()
        {
            this.ResetDevice();
            this.EffectResetDefault();

            this.MatrixReset();
            this.TextureReset();

            this.EffectReset();
        }

        private void ResetDevice()
        {
            // Reset states
            this.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            this.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            // Reset buffers
            this.GraphicsDevice.SetVertexBuffer(null);
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Indices = null;
        }

        #endregion

        #region Draw Primitives

        internal void DrawPrimitives<T>(
            PrimitiveType primitiveType,
            T[] vertexData,
            int vertexOffset,
            int primitiveCount)
            where T : struct, IVertexType
        {
            if (primitiveCount <= 0)
            {
                return;
            }

            this.EffectApply();

            foreach (var pass in this.effectCurrent.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.GraphicsDevice.DrawUserPrimitives(
                    primitiveType,
                    vertexData,
                    vertexOffset,
                    primitiveCount);
            }
        }

        internal void DrawIndexedPrimitives<T>(
            PrimitiveType primitiveType,
            T[] vertexData,
            int vertexOffset,
            int vertexCount,
            short[] indexData,
            int indexOffset,
            int primitiveCount)
            where T : struct, IVertexType
        {
            if (primitiveCount <= 0)
            {
                return;
            }

            this.EffectApply();

            foreach (var pass in this.effectCurrent.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.GraphicsDevice.DrawUserIndexedPrimitives(
                    primitiveType,
                    vertexData,
                    vertexOffset,
                    vertexCount,
                    indexData,
                    indexOffset,
                    primitiveCount);
            }
        }

        #endregion
    }
}

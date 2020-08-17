using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public class CameraRenderer
    {
        private ScriptableRenderContext _context;
        private Camera _camera;
        private const string BufferName = "RenderCamera";
        private CommandBuffer _buffer = new CommandBuffer {name = BufferName};
        private CullingResults _cullingResults;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            _context = context;
            _camera = camera;
            
            if (!Cull()) { return; }
            
            Setup();            
            DrawVisibleGeometry();
            Submit();
        }

        private void DrawVisibleGeometry()
        {
            _context.DrawSkybox(_camera);
        }

        private void Setup()
        {
            _context.SetupCameraProperties(_camera);
            _buffer.ClearRenderTarget(true, true, Color.clear);
            _buffer.BeginSample(BufferName);
            ExecuteBuffer();
        }
        
        private void Submit()
        {
            _buffer.EndSample(BufferName);
            ExecuteBuffer();
            _context.Submit();
        }

        private void ExecuteBuffer()
        {
            _context.ExecuteCommandBuffer(_buffer);
            _buffer.Clear();
        }

        private bool Cull()
        {
            if (_camera.TryGetCullingParameters(out ScriptableCullingParameters param))
            {
                _cullingResults = _context.Cull(ref param);
                return true;
            }
            return false;
        }
    }
}

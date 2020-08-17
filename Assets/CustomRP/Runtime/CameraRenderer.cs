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

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            _context = context;
            _camera = camera;
            
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
    }
}

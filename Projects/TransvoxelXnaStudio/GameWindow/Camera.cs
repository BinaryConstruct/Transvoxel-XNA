using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TransvoxelXnaStudio.GameWindow
{
    public class Camera
    {
        public enum CameraMode
        {
            free = 0,
            chase = 1,
            orbit = 2
        }
        public CameraMode currentCameraMode = CameraMode.free;

        private Vector3 position;
        private Vector3 desiredPosition;
        private Vector3 target;
        private Vector3 desiredTarget;
        private Vector3 offsetDistance;

        private float yaw, pitch, roll;
        private float speed;

        private Matrix cameraRotation;
        public Matrix ViewMatrix, ProjectionMatrix;

        public float Yaw {get { return yaw; }}
        public float Pitch { get { return pitch; } }
        public float Roll { get { return roll; } }
        public Vector3 Position { get { return position; } }

        public Camera()
        {
            ResetCamera();
        }

        public void ResetCamera()
        {
            position = new Vector3(0, 0, 50);
            desiredPosition = position;
            target = new Vector3();
            desiredTarget = target;

            offsetDistance = new Vector3(0, 0, 50);

            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            speed = .3f;

            cameraRotation = Matrix.Identity;
            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, .5f, 500f);       
        }

        public void Update(Matrix chasedObjectsWorld)
        {
            HandleInput();
            UpdateViewMatrix(chasedObjectsWorld);
        }

        private void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //Rotate Camera
            if (keyboardState.IsKeyDown(Keys.J))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    yaw += .02f;
                }
                else
                {
                    //This is to make the panning more prominent in the Chase Camera.
                    yaw += .2f;
                }
            }
            if (keyboardState.IsKeyDown(Keys.L))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    yaw += -.02f;
                }
                else
                {
                    yaw += -.2f;
                }
            }
            if (keyboardState.IsKeyDown(Keys.I))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    pitch += -.02f;
                }
                else
                {
                    pitch += .2f;
                }
            }
            if (keyboardState.IsKeyDown(Keys.K))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    pitch += .02f;
                }
                else
                {
                    pitch += -.2f;
                }
            }
            if (keyboardState.IsKeyDown(Keys.U))
            {
                roll += -.02f;
            }
            if (keyboardState.IsKeyDown(Keys.O))
            {
                roll += .02f;
            }

            //Move Camera
            if (currentCameraMode == CameraMode.free)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    MoveCamera(cameraRotation.Forward);
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    MoveCamera(-cameraRotation.Forward);
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    MoveCamera(-cameraRotation.Right);
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    MoveCamera(cameraRotation.Right);
                }
                if (keyboardState.IsKeyDown(Keys.E))
                {
                    MoveCamera(cameraRotation.Up);
                }
                if (keyboardState.IsKeyDown(Keys.Q))
                {
                    MoveCamera(-cameraRotation.Up);
                }
            }            
        }

        private void MoveCamera(Vector3 addedVector)
        {
            position += speed * addedVector;
        }

        private void UpdateViewMatrix(Matrix chasedObjectsWorld)
        {
            switch (currentCameraMode)
            {
                case CameraMode.free:

                    cameraRotation.Forward.Normalize();
                    cameraRotation.Up.Normalize();
                    cameraRotation.Right.Normalize();

                    cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Right, pitch);
                    cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Up, yaw);
                    cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

                    yaw = 0.0f;
                    pitch = 0.0f;
                    roll = 0.0f;

                    target = position + cameraRotation.Forward;
                    
                    break;

                case CameraMode.chase:

                    cameraRotation.Forward.Normalize();
                    chasedObjectsWorld.Right.Normalize();
                    chasedObjectsWorld.Up.Normalize();

                    cameraRotation = Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);
                    
                    desiredTarget = chasedObjectsWorld.Translation;
                    target = desiredTarget;

                    target += chasedObjectsWorld.Right * yaw;
                    target += chasedObjectsWorld.Up * pitch;
                    
                    desiredPosition = Vector3.Transform(offsetDistance, chasedObjectsWorld);
                    position = Vector3.SmoothStep(position, desiredPosition, .15f);
                    
                    yaw = MathHelper.SmoothStep(yaw, 0f, .1f);
                    pitch = MathHelper.SmoothStep(pitch, 0f, .1f);
                    roll = MathHelper.SmoothStep(roll, 0f, .2f);

                    break;

                case CameraMode.orbit:

                    cameraRotation.Forward.Normalize();
                    
                    cameraRotation = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw) * Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

                    desiredPosition = Vector3.Transform(offsetDistance, cameraRotation);
                    desiredPosition += chasedObjectsWorld.Translation;
                    position = desiredPosition;

                    target = chasedObjectsWorld.Translation;

                    roll = MathHelper.SmoothStep(roll, 0f, .2f);                                                         

                    break;
            }

            //We'll always use this line of code to set up the View Matrix.
            ViewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
        }

        //This cycles through the different camera modes.
        public void SwitchCameraMode()
        {
            ResetCamera();

            currentCameraMode++;

            if ((int)currentCameraMode > 2)
            {
                currentCameraMode = 0;
            }
        }
    }
}
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
using TransvoxelXnaStudio.Framework;

namespace TransvoxelXnaStudio.GameWindow
{
    public class Camera
    {
        private readonly GraphicsDevice _graphicsDevice;

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

        public float Yaw { get { return yaw; } }
        public float Pitch { get { return pitch; } }
        public float Roll { get { return roll; } }
        public Vector3 Position { get { return position; } }

        public Camera(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            ResetCamera();
        }

        public void ResetCamera(bool resetPosition)
        {
            if (resetPosition)
            {

                position = new Vector3(0, 0, 50);
                desiredPosition = position;
                target = new Vector3();
                desiredTarget = target;

                offsetDistance = new Vector3(0, 0, 50);
            }
            ResetCamera();
        }
        public void ResetCamera()
        {

            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            speed = .3f;

            cameraRotation = Matrix.Identity;
            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), _graphicsDevice.Viewport.AspectRatio, 0.01f, 1000f);
        }

        public void Update(Matrix chasedObjectsWorld)
        {
            HandleInput();
            UpdateViewMatrix(chasedObjectsWorld);
        }

        private void HandleInput()
        {
            // Don't handle input if we don't have focus
            if (!MouseKeyboard.ApplicationIsActivated())
            {
                return;
            }

            //Rotate Camera
            float rotationSpeed = 0.05f;
            float movementSpeed = 1.5f;

            if (MouseKeyboard.IsKeyToggled(Keys.Shift))
            {
                movementSpeed *= 5;
                rotationSpeed *= 5;

            }

            if (MouseKeyboard.IsKeyDown(Keys.J))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    yaw += rotationSpeed;
                }
                else
                {
                    //This is to make the panning more prominent in the Chase Camera.
                    yaw += rotationSpeed;
                }
            }
            if (MouseKeyboard.IsKeyDown(Keys.L))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    yaw += -rotationSpeed;
                }
                else
                {
                    yaw += -rotationSpeed;
                }
            }
            if (MouseKeyboard.IsKeyDown(Keys.I))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    pitch += -rotationSpeed;
                }
                else
                {
                    pitch += rotationSpeed;
                }
            }
            if (MouseKeyboard.IsKeyDown(Keys.K))
            {
                if (currentCameraMode != CameraMode.chase)
                {
                    pitch += rotationSpeed;
                }
                else
                {
                    pitch += -rotationSpeed;
                }
            }
            if (MouseKeyboard.IsKeyDown(Keys.U))
            {
                roll += -rotationSpeed;
            }
            if (MouseKeyboard.IsKeyDown(Keys.O))
            {
                roll += rotationSpeed;
            }

            //Move Camera
            if (currentCameraMode == CameraMode.free)
            {
                if (MouseKeyboard.IsKeyDown(Keys.W))
                {
                    MoveCamera(cameraRotation.Forward * movementSpeed);
                }
                if (MouseKeyboard.IsKeyDown(Keys.S))
                {
                    MoveCamera(-cameraRotation.Forward * movementSpeed);
                }
                if (MouseKeyboard.IsKeyDown(Keys.A))
                {
                    MoveCamera(-cameraRotation.Right * movementSpeed);
                }
                if (MouseKeyboard.IsKeyDown(Keys.D))
                {
                    MoveCamera(cameraRotation.Right * movementSpeed);
                }
                if (MouseKeyboard.IsKeyDown(Keys.E))
                {
                    MoveCamera(cameraRotation.Up * movementSpeed);
                }
                if (MouseKeyboard.IsKeyDown(Keys.Q))
                {
                    MoveCamera(-cameraRotation.Up * movementSpeed);
                }
            }
        }

        private void MoveCamera(Vector3 addedVector)
        {
            position += speed * addedVector;
        }

        public void SetFreeCamPosition(Vector3 pos, Quaternion rotation)
        {
            cameraRotation = Matrix.CreateFromQuaternion(rotation);

            cameraRotation.Forward.Normalize();
            cameraRotation.Up.Normalize();
            cameraRotation.Right.Normalize();

            SetFreeCamPosition(pos);
        }
        public void SetFreeCamPosition(Vector3 pos)
        {
            position = pos;
            desiredPosition = position;
            target = position + cameraRotation.Forward;
            desiredTarget = target;
            ViewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
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
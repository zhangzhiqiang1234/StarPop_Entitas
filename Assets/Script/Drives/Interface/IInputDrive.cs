public interface IInputDrive
{
    bool GetMouseButtonDown(int keyCode);
    MVector3D MousePosition();
    MVector3D ScreenToWorldPoint(MVector3D vector3D);
}

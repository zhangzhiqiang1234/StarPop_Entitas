public interface ITimeDrive
{

    float GetDeltaTime();
    float GetTime();
    float GetUnscaledDeltaTime();
    float GetUnscaledTime();
    float GetTimeScale();
    void SetTimeScale(float scale);
}

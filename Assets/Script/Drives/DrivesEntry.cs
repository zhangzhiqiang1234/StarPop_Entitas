public class DrivesEntry
{
    public readonly ITimeDrive Time;
    public readonly ILogDrive Log;
    public readonly IInputDrive Input;

    public DrivesEntry(ITimeDrive time,ILogDrive log, IInputDrive input)
    {
        Time = time;
        Log = log;
        Input = input;
    }
}

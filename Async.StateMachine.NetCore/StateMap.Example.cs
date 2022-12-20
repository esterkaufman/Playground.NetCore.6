
public class StateMap_Example
{
    public static StateMachineConfig<string,string> StateConfig = new StateMachineConfig<string,string>("Ready",
        new List<State<string,string>>
        {
            new ("Ready", new List<Transition<string, string>>
                {
                    new ("Processing",  "Process")
                }
            ),
            new ("Process", new List<Transition<string, string>>
                {
                    new ("Success",  "Result"),
                    new ("Failure",  "Error"),
                }
            ),
            new ("Error", new List<Transition<string, string>>
                {
                    new ("Retry",  "Process")
                }
            ),
            new ("Result", new List<Transition<string, string>>
                {
                    new ("Finish",  "Ready")
                }
            )},default,default);
    
}
using Hwdtech;

namespace SpaceBattle.Lib;
public class SoftStop: ICommand
{
    private ServerThread _st;
    //private BlockingCollection<ICommand> _q;
    public SoftStop(ServerThread st){
        _st = st;
        
    }
    public void Execute()
    {
    _st.UpdateBehaviour( () =>{
        if (_st.GetQueue().Count() > 0) 
        {
            var cmd = _st.GetQueue().Take();
            try
            {
                cmd.Execute();
            } 
            catch (Exception e) 
            {
                IoC.Resolve<ICommand>("ExceptionHandler.Handle", cmd, e).Execute();
            }
        } 
        else 
        {
            _st.Stop();
        }
    });
    }
}
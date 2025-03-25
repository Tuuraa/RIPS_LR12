namespace LR12.Web
{
    public class ComputerComponentApiClient(HttpClient httpClient)
    {
        public async Task<ComputerComponent[]> GetComponentsAsync(int maxItems = 10, CancellationToken cancellationToken = default)
        {

            return null;
        }
    }

}
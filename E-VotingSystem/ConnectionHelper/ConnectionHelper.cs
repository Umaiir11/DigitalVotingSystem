public class CmConnectionHelper
{
    public string FncGetConnectionString()
    {
        return "Data Source=MUHAMMAD-UMAIR\\AISONESQL;Initial Catalog=EVoting;Integrated Security=True";
    }

    public void WriteToFile(string l_Text)
    {
        try
        {
            string l_Path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"EVoting_Exception.txt");
            File.WriteAllText(l_Path,l_Text);          
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to the file: " + ex.Message);
        }
    }
}
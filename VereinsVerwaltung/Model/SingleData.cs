namespace VereinsVerwaltung.Model
{
    public class SingleData //Our fancy data model
    {
        public string? Header { get; set; }  //Header from column    
        public dynamic? Inhalt { get; set; } //Value from field
        public int InterneId { get; set; }  //Internal ID to keep track of each row 
        public string? FileName { get; set; }    //FileName mainly needed for column merging
    }
}

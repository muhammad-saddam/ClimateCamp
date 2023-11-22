namespace ClimateCamp.Common.Dto
{
    public class DDLDto<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PicturePath { get; internal set; }
    }
}

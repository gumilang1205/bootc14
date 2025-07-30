// public interface IDisposable
// {
//     void Dispose();
// }

using (FileStream fs = new FileStream("myFile.txt", FileMode.Open))
{

}
// GC.SuppressFinalize(this);
// try
// {
//     File.Delete(FilePath);
// }
// catch (Exception ex)
// {
// }
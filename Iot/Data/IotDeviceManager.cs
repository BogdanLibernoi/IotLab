using System.IO.Ports;

namespace Iot.Data
{
    public class IotDeviceManager
    {
        SerialPort _serialPort;
        static string full_buffer = "";
        int counter = 1;

        public IotDeviceManager()
        {
            Init();
        }

        public async void Init()
        {
            _serialPort = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            _serialPort.DataReceived += DataBaseWriterAsync;
        }

        private async void DataBaseWriterAsync(object sender, SerialDataReceivedEventArgs e)
        {

            int numBytes = _serialPort.BytesToRead;
            byte[] buffer = new byte[numBytes];
            _serialPort.Read(buffer, 0, numBytes);
            string stroka = System.Text.Encoding.Default.GetString(buffer);
            full_buffer += stroka;
            var res = ParseBuffer();
            if (res != null)
            {

                full_buffer = "";
                using (AppDbContext dbcontext = new AppDbContext())
                {
                    DeviceData data = new DeviceData
                    {
                        Id = counter++,
                        Date = DateTime.Now.ToString(),
                        Tempeture = res.Item1,
                        Humidity = res.Item2
                    };
                    dbcontext.Add(data);
                    dbcontext.SaveChanges();
                }
            }


        }


        static Tuple<double, double> ParseBuffer()
        {
            if (full_buffer.Contains("<<") && full_buffer.Length > 14)
            {
                int startindexTemp = full_buffer.IndexOf("<<") + 2;
                int endIndexTemp = full_buffer.IndexOf("||||");
                int endIndexHumad = full_buffer.IndexOf(">>");
                string strtemp = full_buffer.Substring(startindexTemp, endIndexTemp - startindexTemp);
                string strhum = full_buffer.Substring(endIndexTemp + 4, endIndexHumad - endIndexTemp - 4);
                double temp = double.Parse(strtemp.Replace(".", ","));
                double hum = double.Parse(strhum.Replace(".", ","));
                return new Tuple<double, double>(temp, hum);
            }
            return null;
        }


    }
}


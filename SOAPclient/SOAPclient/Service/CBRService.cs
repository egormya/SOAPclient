using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SOAPclient.ru.cbr.www;

namespace SOAPclient
{
    public class CBRService : INotifyPropertyChanged
    {
        private ru.cbr.www.DailyInfo _cbrClient;
        public decimal LastQueriedRate
        {
            get
            {
                return lastQueriedRate;
            }

            set
            {
                if (lastQueriedRate == value) return;
                lastQueriedRate = value;
                NotifyPropertyChanged("LastQueriedRate");
            }
        }

        // обеспечивает доступ к веб-сервису
        public ru.cbr.www.DailyInfo Cbr
        {
            get
            {
                if (_cbrClient == null)
                {
                    _cbrClient = new DailyInfo();
                }
                return _cbrClient;
            }
        }

        private static decimal ExtractCurrencyRate(DataSet ds, string currencyCode)
        {
            if (ds == null)
                throw new ArgumentNullException("ds", "Параметр ds не может быть null.");

            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentNullException("currencyCode", "Параметр currencyCode не может быть null.");

            DataTable dt = ds.Tables["ValuteCursOnDate"];

            DataRow[] rows = dt.Select(string.Format("VchCode=\'{0}\'", currencyCode));

            if (rows.Length > 0)
            {
                decimal result;
                if (decimal.TryParse(rows[0]["Vcurs"].ToString(), out result))
                    return result;
                throw new InvalidCastException("От службы ожидалось значение курса валют.");

            }
            throw new KeyNotFoundException("Для заданной валюты не найден курс.");

        }
        public decimal GetCurrencyRateOnDate(DateTime onDate, string currencyCode)
        {
            var ds = _cbrClient.GetCursOnDate(onDate);

            return ExtractCurrencyRate(ds, currencyCode);
        }
        public decimal lastQueriedRate;


        public void AsyncGetCurrencyRateOnDate(DateTime dateTime, string currencyCode)
        {
            Cbr.GetCursOnDateCompleted += new GetCursOnDateCompletedEventHandler(_cbrClient_GetCursOnDateCompleted);
            Cbr.GetCursOnDateAsync(dateTime, currencyCode);
        }
        void _cbrClient_GetCursOnDateCompleted(object sender, GetCursOnDateCompletedEventArgs e)
        {
            LastQueriedRate = ExtractCurrencyRate(e.Result, (string)e.UserState);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

using System;
using NHSData.DataObjects;

namespace NHSData.Messages
{
    public class DataRowMessage
    {
        public Type RowType { get; }
        public IDataRow Row { get; }

        public DataRowMessage(Type type, IDataRow row)
        {
            RowType = type;
            Row = row;
        }
    }
}
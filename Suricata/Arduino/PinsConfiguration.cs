using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace Arduino
{
    [DataContract]
    public class PinConfigurationAnalogOutput
    {
        [DataMember]
        public Arduino.Firmata.Types.Pins Pin
        {
            get;
            set;
        }
    }

    [DataContract]
    public class PinConfigurationDigitalPort
    {
        [DataMember]
        public Arduino.Firmata.Types.Pins Pin
        {
            get;
            set;
        }

        [DataMember]
        public Arduino.Firmata.Types.PinMode Mode
        {
            get;
            set;
        }

        [DataMember]
        public Arduino.Firmata.Types.PinDigitalValue OUTPUTInitialValue
        {
            get;
            set;
        }

        [DataMember]
        public byte PWMInitialValue
        {
            get;
            set;
        }
    }

    [DataContract]
    public class PinConfigurationCollection
    {
        #region Аналоговые входы
        [DataMember]
        public PinConfigurationAnalogOutput[] Analog
        {
            get;
            set;
        }
        #endregion

        #region Цифровые порты
        [DataMember]
        public PinConfigurationDigitalPort[] Digital
        {
            get;
            set;
        }
        #endregion
    }
}

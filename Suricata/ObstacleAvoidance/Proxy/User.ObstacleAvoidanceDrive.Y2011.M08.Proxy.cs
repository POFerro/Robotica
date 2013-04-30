//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Reflection.AssemblyVersionAttribute("0.0.0.0")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Proxy, SourceAssemblyKey="User.ObstacleAvoidanceDrive.Y2011.M08, Version=0.0.0.0, Culture=neutral, PublicKe" +
    "yToken=e3a6d1d8ea7297f8")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy {
    
    
    /// <summary>
    ///            Service state
    ///            </summary>
    [global::Microsoft.Dss.Core.Attributes.DataContractAttribute(Namespace="http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html")]
    [global::System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html", ElementName="ObstacleAvoidanceDriveState")]
    [global::System.ComponentModel.DescriptionAttribute("Service state")]
    public class ObstacleAvoidanceDriveState : global::Microsoft.Dss.Core.IDssSerializable, global::System.ICloneable {
        
        public ObstacleAvoidanceDriveState() {
        }
        
        private double _RobotWidth;
        
        /// <summary>
        ///            Gets or sets robot width in meters
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Gets or sets robot width in meters")]
        public double RobotWidth {
            get {
                return this._RobotWidth;
            }
            set {
                this._RobotWidth = value;
            }
        }
        
        private double _MaxPowerPerWheel;
        
        /// <summary>
        ///            Gets or sets max power allowed per wheel
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Gets or sets max power allowed per wheel")]
        public double MaxPowerPerWheel {
            get {
                return this._MaxPowerPerWheel;
            }
            set {
                this._MaxPowerPerWheel = value;
            }
        }
        
        private double _MaxPowerDifferenceBetweenWheels;
        
        /// <summary>
        ///            When turning, we want to eliminate drastic differences between wheel power settings.
        ///            not doing so may result in unpractically fast rotation that essentially makes robot 
        ///            uncontrollable
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("When turning, we want to eliminate drastic differences between wheel power settin" +
            "gs. not doing so may result in unpractically fast rotation that essentially make" +
            "s robot uncontrollable")]
        public double MaxPowerDifferenceBetweenWheels {
            get {
                return this._MaxPowerDifferenceBetweenWheels;
            }
            set {
                this._MaxPowerDifferenceBetweenWheels = value;
            }
        }
        
        private double _MinRotationSpeed;
        
        /// <summary>
        ///            Gets or sets the minimum rotation speed
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Gets or sets the minimum rotation speed")]
        public double MinRotationSpeed {
            get {
                return this._MinRotationSpeed;
            }
            set {
                this._MinRotationSpeed = value;
            }
        }
        
        private global::Microsoft.Robotics.PhysicalModel.Proxy.Vector3 _DepthCameraPosition;
        
        /// <summary>
        ///            Gets or sets the depth camera position relative to the floor plane 
        ///            and the projection of the center of mass of the robot to the floor plane
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Gets or sets the depth camera position relative to the floor plane and the projec" +
            "tion of the center of mass of the robot to the floor plane")]
        public global::Microsoft.Robotics.PhysicalModel.Proxy.Vector3 DepthCameraPosition {
            get {
                return this._DepthCameraPosition;
            }
            set {
                this._DepthCameraPosition = value;
            }
        }
        
        private global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController _Controller;
        
        /// <summary>
        ///            Gets or sets the obstacle avoidance controller state
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Gets or sets the obstacle avoidance controller state")]
        public global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController Controller {
            get {
                return this._Controller;
            }
            set {
                this._Controller = value;
            }
        }
        
        private double _MaxDeltaPower;
        
        /// <summary>
        ///            Gets or sets the maximum allowed change in Power from one call to SetPower to the next
        ///            Smaller numbers will cause smoother accelrations, but can also increase chance of collision with 
        ///            obstacles
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Gets or sets the maximum allowed change in Power from one call to SetPower to the" +
            " next Smaller numbers will cause smoother accelrations, but can also increase ch" +
            "ance of collision with obstacles")]
        public double MaxDeltaPower {
            get {
                return this._MaxDeltaPower;
            }
            set {
                this._MaxDeltaPower = value;
            }
        }
        
        /// <summary>
        ///Copies the data member values of the current ObstacleAvoidanceDriveState to the specified target object
        ///</summary>
        ///<param name="target">target object (must be an instance of)</param>
        public virtual void CopyTo(Microsoft.Dss.Core.IDssSerializable target) {
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState typedTarget = ((global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState)(target));
            typedTarget._RobotWidth = this._RobotWidth;
            typedTarget._MaxPowerPerWheel = this._MaxPowerPerWheel;
            typedTarget._MaxPowerDifferenceBetweenWheels = this._MaxPowerDifferenceBetweenWheels;
            typedTarget._MinRotationSpeed = this._MinRotationSpeed;
            typedTarget._DepthCameraPosition = this._DepthCameraPosition;
            if ((this._Controller != null)) {
                global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController tmp = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController();
                ((Microsoft.Dss.Core.IDssSerializable)(this._Controller)).CopyTo(((Microsoft.Dss.Core.IDssSerializable)(tmp)));
                typedTarget._Controller = tmp;
            }
            typedTarget._MaxDeltaPower = this._MaxDeltaPower;
        }
        
        /// <summary>
        ///Clones ObstacleAvoidanceDriveState
        ///</summary>
        ///<returns>cloned value</returns>
        public virtual object Clone() {
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState target0 = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState();
            this.CopyTo(target0);
            return target0;
        }
        
        /// <summary>
        ///Serializes the data member values of the current ObstacleAvoidanceDriveState to the specified writer
        ///</summary>
        ///<param name="writer">the writer to which to serialize</param>
        public virtual void Serialize(System.IO.BinaryWriter writer) {
            writer.Write(this._RobotWidth);
            writer.Write(this._MaxPowerPerWheel);
            writer.Write(this._MaxPowerDifferenceBetweenWheels);
            writer.Write(this._MinRotationSpeed);
            ((Microsoft.Dss.Core.IDssSerializable)(this._DepthCameraPosition)).Serialize(writer);
            if ((this._Controller == null)) {
                writer.Write(((byte)(0)));
            }
            else {
                writer.Write(((byte)(1)));
                ((Microsoft.Dss.Core.IDssSerializable)(this._Controller)).Serialize(writer);
            }
            writer.Write(this._MaxDeltaPower);
        }
        
        /// <summary>
        ///Deserializes ObstacleAvoidanceDriveState
        ///</summary>
        ///<param name="reader">the reader from which to deserialize</param>
        ///<returns>deserialized ObstacleAvoidanceDriveState</returns>
        public virtual object Deserialize(System.IO.BinaryReader reader) {
            this._RobotWidth = reader.ReadDouble();
            this._MaxPowerPerWheel = reader.ReadDouble();
            this._MaxPowerDifferenceBetweenWheels = reader.ReadDouble();
            this._MinRotationSpeed = reader.ReadDouble();
            this._DepthCameraPosition = ((global::Microsoft.Robotics.PhysicalModel.Proxy.Vector3)(((Microsoft.Dss.Core.IDssSerializable)(new global::Microsoft.Robotics.PhysicalModel.Proxy.Vector3())).Deserialize(reader)));
            if ((reader.ReadByte() != 0)) {
                this._Controller = ((global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController)(((Microsoft.Dss.Core.IDssSerializable)(new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController())).Deserialize(reader)));
            }
            this._MaxDeltaPower = reader.ReadDouble();
            return this;
        }
    }
    
    /// <summary>
    ///            Simple PID controller state and behavior
    ///            </summary>
    [global::Microsoft.Dss.Core.Attributes.DataContractAttribute(Namespace="http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html")]
    [global::System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html", ElementName="PIDController")]
    [global::System.ComponentModel.DescriptionAttribute("Simple PID controller state and behavior")]
    public class PIDController : global::Microsoft.Dss.Core.IDssSerializable, global::System.ICloneable {
        
        public PIDController() {
        }
        
        private double _Ki;
        
        /// <summary>
        ///            Integral constant
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Integral constant")]
        public double Ki {
            get {
                return this._Ki;
            }
            set {
                this._Ki = value;
            }
        }
        
        private double _Kp;
        
        /// <summary>
        ///             Proportional constant
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Proportional constant")]
        public double Kp {
            get {
                return this._Kp;
            }
            set {
                this._Kp = value;
            }
        }
        
        private double _Kd;
        
        /// <summary>
        ///             Derivative constant
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Derivative constant")]
        public double Kd {
            get {
                return this._Kd;
            }
            set {
                this._Kd = value;
            }
        }
        
        /// <summary>
        ///Copies the data member values of the current PIDController to the specified target object
        ///</summary>
        ///<param name="target">target object (must be an instance of)</param>
        public virtual void CopyTo(Microsoft.Dss.Core.IDssSerializable target) {
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController typedTarget = ((global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController)(target));
            typedTarget._Ki = this._Ki;
            typedTarget._Kp = this._Kp;
            typedTarget._Kd = this._Kd;
        }
        
        /// <summary>
        ///Clones PIDController
        ///</summary>
        ///<returns>cloned value</returns>
        public virtual object Clone() {
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController target0 = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.PIDController();
            this.CopyTo(target0);
            return target0;
        }
        
        /// <summary>
        ///Serializes the data member values of the current PIDController to the specified writer
        ///</summary>
        ///<param name="writer">the writer to which to serialize</param>
        public virtual void Serialize(System.IO.BinaryWriter writer) {
            writer.Write(this._Ki);
            writer.Write(this._Kp);
            writer.Write(this._Kd);
        }
        
        /// <summary>
        ///Deserializes PIDController
        ///</summary>
        ///<param name="reader">the reader from which to deserialize</param>
        ///<returns>deserialized PIDController</returns>
        public virtual object Deserialize(System.IO.BinaryReader reader) {
            this._Ki = reader.ReadDouble();
            this._Kp = reader.ReadDouble();
            this._Kd = reader.ReadDouble();
            return this;
        }
    }
    
    /// <summary>
    ///            Partner names
    ///            </summary>
    [global::Microsoft.Dss.Core.Attributes.DataContractAttribute(Namespace="http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html")]
    [global::System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html", ElementName="Partners")]
    [global::System.ComponentModel.DescriptionAttribute("Partner names")]
    public class Partners : global::Microsoft.Dss.Core.IDssSerializable, global::System.ICloneable {
        
        public Partners() {
        }
        
        /// <summary>
        ///            Drive service
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Drive service")]
        public const string Drive = "Drive";
        
        /// <summary>
        ///            Depth cam service
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Depth cam service")]
        public const string DepthCamSensor = "DepthCamera";
        
        /// <summary>
        ///            IR sensor array
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("IR sensor array")]
        public const string InfraredSensorArray = "InfraredSensorArray";
        
        /// <summary>
        ///            Sonar analog sensors
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Sonar analog sensors")]
        public const string SonarSensorArray = "SonarSensorArray";
        
        /// <summary>
        ///            Time we are willing to wait for each partner to start
        ///            </summary>
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Time we are willing to wait for each partner to start")]
        public const int PartnerEnumerationTimeoutInSeconds = 120;
        
        /// <summary>
        ///Copies the data member values of the current Partners to the specified target object
        ///</summary>
        ///<param name="target">target object (must be an instance of)</param>
        public virtual void CopyTo(Microsoft.Dss.Core.IDssSerializable target) {
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Partners typedTarget = ((global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Partners)(target));
        }
        
        /// <summary>
        ///Clones Partners
        ///</summary>
        ///<returns>cloned value</returns>
        public virtual object Clone() {
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Partners target0 = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Partners();
            this.CopyTo(target0);
            return target0;
        }
        
        /// <summary>
        ///Serializes the data member values of the current Partners to the specified writer
        ///</summary>
        ///<param name="writer">the writer to which to serialize</param>
        public virtual void Serialize(System.IO.BinaryWriter writer) {
        }
        
        /// <summary>
        ///Deserializes Partners
        ///</summary>
        ///<param name="reader">the reader from which to deserialize</param>
        ///<returns>deserialized Partners</returns>
        public virtual object Deserialize(System.IO.BinaryReader reader) {
            return this;
        }
    }
    
    /// <summary>
    ///            Get operation
    ///            </summary>
    [global::System.ComponentModel.DescriptionAttribute("Get operation")]
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class Get : global::Microsoft.Dss.ServiceModel.Dssp.Get<global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType, global:: Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState>> {
        
        public Get() {
        }
        
        public Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body) : 
                base(body) {
        }
        
        public Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body, global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState> responsePort) : 
                base(body, responsePort) {
        }
    }
    
    /// <summary>
    ///            Operations port
    ///            </summary>
    [global::System.ComponentModel.DescriptionAttribute("Operations port")]
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class ObstacleAvoidanceDriveOperationsPort : global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get, global:: Microsoft.Dss.Core.DsspHttp.HttpQuery, global:: Microsoft.Dss.Core.DsspHttp.HttpGet, global:: Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet, global:: Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup, global:: Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop> {
        
        public ObstacleAvoidanceDriveOperationsPort() {
        }
        
        public virtual global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState> Get() {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get operation = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Get(out global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            operation = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveState> Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get operation = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body, out global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            operation = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.Core.DsspHttp.HttpResponseType, global::W3C.Soap.Fault> HttpQuery() {
            global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType body = new global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType();
            global::Microsoft.Dss.Core.DsspHttp.HttpQuery operation = new global::Microsoft.Dss.Core.DsspHttp.HttpQuery(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice HttpQuery(out global::Microsoft.Dss.Core.DsspHttp.HttpQuery operation) {
            global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType body = new global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType();
            operation = new global::Microsoft.Dss.Core.DsspHttp.HttpQuery(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.Core.DsspHttp.HttpResponseType, global::W3C.Soap.Fault> HttpQuery(global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType();
            }
            global::Microsoft.Dss.Core.DsspHttp.HttpQuery operation = new global::Microsoft.Dss.Core.DsspHttp.HttpQuery(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice HttpQuery(global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType body, out global::Microsoft.Dss.Core.DsspHttp.HttpQuery operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.Core.DsspHttp.HttpQueryRequestType();
            }
            operation = new global::Microsoft.Dss.Core.DsspHttp.HttpQuery(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.Core.DsspHttp.HttpResponseType, global::W3C.Soap.Fault> HttpGet() {
            global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType body = new global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType();
            global::Microsoft.Dss.Core.DsspHttp.HttpGet operation = new global::Microsoft.Dss.Core.DsspHttp.HttpGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice HttpGet(out global::Microsoft.Dss.Core.DsspHttp.HttpGet operation) {
            global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType body = new global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType();
            operation = new global::Microsoft.Dss.Core.DsspHttp.HttpGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.Core.DsspHttp.HttpResponseType, global::W3C.Soap.Fault> HttpGet(global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType();
            }
            global::Microsoft.Dss.Core.DsspHttp.HttpGet operation = new global::Microsoft.Dss.Core.DsspHttp.HttpGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice HttpGet(global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType body, out global::Microsoft.Dss.Core.DsspHttp.HttpGet operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.Core.DsspHttp.HttpGetRequestType();
            }
            operation = new global::Microsoft.Dss.Core.DsspHttp.HttpGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::W3C.Soap.Fault, object> DsspDefaultGet() {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultGet(out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::W3C.Soap.Fault, object> DsspDefaultGet(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultGet(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body, out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultGet(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.LookupResponse, global::W3C.Soap.Fault> DsspDefaultLookup() {
            global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultLookup(out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.LookupResponse, global::W3C.Soap.Fault> DsspDefaultLookup(global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            }
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultLookup(global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body, out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            }
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultDropResponseType, global::W3C.Soap.Fault> DsspDefaultDrop() {
            global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultDrop(out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultDropResponseType, global::W3C.Soap.Fault> DsspDefaultDrop(global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            }
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultDrop(global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body, out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            }
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
    }
    
    /// <summary>
    ///            Obstacle Avoidance Drive Service
    ///            </summary>
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    [global::System.ComponentModel.DescriptionAttribute("Semi autonomous drive service utilizing a depthcam and proximity sensors for obst" +
        "acle avoidance and open space explore")]
    [global::System.ComponentModel.DisplayNameAttribute("(User) Obstacle Avoidance Drive")]
    public class Contract {
        
        public const string Identifier = "http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html";
        
        /// <summary>Creates a new instance of the service.</summary>
        /// <param name="constructorServicePort">Service constructor port</param>
        /// <param name="service">service path</param>
        /// <param name="partners">the partners of the service instance</param>
        /// <returns>create service response port</returns>
        public static global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> CreateService(global::Microsoft.Dss.Services.Constructor.ConstructorPort constructorServicePort, string service, params Microsoft.Dss.ServiceModel.Dssp.PartnerType[] partners) {
            global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> result = new global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse>();
            global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType serviceInfo = new global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType("http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html", service);
            if ((partners != null)) {
                serviceInfo.PartnerList = new System.Collections.Generic.List<Microsoft.Dss.ServiceModel.Dssp.PartnerType>(partners);
            }
            global::Microsoft.Dss.Services.Constructor.Create create = new global::Microsoft.Dss.Services.Constructor.Create(serviceInfo, result);
            constructorServicePort.Post(create);
            return result;
        }
        
        /// <summary>Creates a new instance of the service.</summary>
        /// <param name="constructorServicePort">Service constructor port</param>
        /// <param name="partners">the partners of the service instance</param>
        /// <returns>create service response port</returns>
        public static global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> CreateService(global::Microsoft.Dss.Services.Constructor.ConstructorPort constructorServicePort, params Microsoft.Dss.ServiceModel.Dssp.PartnerType[] partners) {
            global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> result = new global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse>();
            global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType serviceInfo = new global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType("http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html", null);
            if ((partners != null)) {
                serviceInfo.PartnerList = new System.Collections.Generic.List<Microsoft.Dss.ServiceModel.Dssp.PartnerType>(partners);
            }
            global::Microsoft.Dss.Services.Constructor.Create create = new global::Microsoft.Dss.Services.Constructor.Create(serviceInfo, result);
            constructorServicePort.Post(create);
            return result;
        }
    }
    
    public class CombinedOperationsPort : global::Microsoft.Dss.Core.DssCombinedOperationsPort {
        
        public CombinedOperationsPort() {
            this.ObstacleAvoidanceDriveOperationsPort = new global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveOperationsPort();
            this.DriveOperations = new global::Microsoft.Robotics.Services.Drive.Proxy.DriveOperations();
            base.Initialize(new global::Microsoft.Dss.Core.DssOperationsPortMetadata(this.ObstacleAvoidanceDriveOperationsPort, "http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html", "ObstacleAvoidanceDriveOperationsPort", ""), new global::Microsoft.Dss.Core.DssOperationsPortMetadata(this.DriveOperations, "http://schemas.microsoft.com/robotics/2006/05/drive.html", "DriveOperations", "/genericobstacleavoidancedrive"));
        }
        
        public global::Microsoft.Robotics.Services.ObstacleAvoidanceDrive.Proxy.ObstacleAvoidanceDriveOperationsPort ObstacleAvoidanceDriveOperationsPort;
        
        public global::Microsoft.Robotics.Services.Drive.Proxy.DriveOperations DriveOperations;
    }
}

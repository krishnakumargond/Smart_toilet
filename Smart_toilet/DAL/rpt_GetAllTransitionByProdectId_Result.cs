//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Smart_toilet.DAL
{
    using System;
    
    public partial class rpt_GetAllTransitionByProdectId_Result
    {
        public int TransitionId { get; set; }
        public string EnergyId { get; set; }
        public string DeviceId { get; set; }
        public string RPhaseVolt { get; set; }
        public string YPhaseVolt { get; set; }
        public string BPhaseVolt { get; set; }
        public string RPhaseCurnt { get; set; }
        public string YPhaseCurnt { get; set; }
        public string BPhaseCurnt { get; set; }
        public string RPhaseKw { get; set; }
        public string YPhaseKw { get; set; }
        public string BPhaseKw { get; set; }
        public string TotalKwhUnit { get; set; }
        public string TotalKwhLoad { get; set; }
        public string PwrFactor { get; set; }
        public string PresentFrqncy { get; set; }
        public Nullable<int> TypeOfData { get; set; }
        public string FuelSensor1Val { get; set; }
        public string FuelSensor2Val { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string TDate { get; set; }
        public Nullable<int> TypeId { get; set; }
        public string DgUnit { get; set; }
    }
}
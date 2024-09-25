namespace VTIWindowsControlLibrary.Classes.IO.SignalConverters
{
    /// <summary>
    /// Signal converter for MSI-brand pressure transducers
    /// </summary>
    public class MsiSignal : LinearSignal
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="MsiSignal">MsiSignal</see>
        ///// class providing only the <see cref="LinearSignal.FullScale">FullScale</see> range of the transducer.
        ///// </summary>
        //public MsiSignal(Double FullScale)
        //    : base(FullScale, 0, 1, 5)
        //{ }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="MsiSignal">MsiSignal</see>
        ///// class providing both the <see cref="LinearSignal.FullScale">FullScale</see> range of the transducer
        ///// and a <see cref="LinearSignal.Offset">Offset</see>.
        ///// </summary>
        //public MsiSignal(Double FullScale, Double Offset)
        //    : base(FullScale, Offset, 1, 5)
        //{ }

        //public MsiSignal(NumericParameter FullScale)
        //    : base()
        //{
        //    base.FullScaleParameter = FullScale;
        //    base.Offset = 0;
        //    base.InputMaximum = 1;
        //    base.InputMaximum = 5;
        //}

        //public MsiSignal(NumericParameter FullScale, NumericParameter Offset)
        //    : base()
        //{
        //    base.FullScaleParameter = FullScale;
        //    base.OffsetParameter = Offset;
        //    base.InputMaximum = 1;
        //    base.InputMaximum = 5;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="MsiSignal">MsiSignal</see>
        /// </summary>
        public MsiSignal()
        {
            Offset = 0;
            InputMinimum = 1;
            InputMaximum = 5;
        }
    }
}
using System;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Structs
{
    public struct BLOB
    {
        public int Length;
        public IntPtr Data;

        /// <summary>
        /// Code Should Compile at warning level4 without any warnings, 
        /// However this struct will give us Warning CS0649: Field [Fieldname] 
        /// is never assigned to, and will always have its default value
        /// You can disable CS0649 in the project options but that will disable
        /// the warning for the whole project, it's a nice warning and we do want 
        /// it in other places so we make a nice dummy function to keep the compiler
        /// happy.        
        /// </summary>
        private void FixCS0649()
        {
            Length = 0;
            Data = IntPtr.Zero;
        }
    }
}

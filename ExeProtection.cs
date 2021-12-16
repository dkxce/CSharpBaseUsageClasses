/*************************************
*                                    *
*  Milok Zbrozek <milokz@gmail.com>  *
*                                    * 
*************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Management;
using System.Security.Cryptography;
using System.Security;

namespace dkxce
{
    /// <summary>
    ///     string machineID;
    ///     dkxce.ExeProtectStandard.Validity valid = dkxce.ExeProtectStandard.ValidateMachine(out machineID);
    ///     if((valid != dkxce.ExeProtect.Validity.Empty) && (valid != dkxce.ExeProtect.Validity.Invalid)) return;
    /// </summary>
    public class ExeProtectStandard
    {
        public static string GetMachineID() { return ExeProtect.GetMachineID(ExeProtect.Defaults.Info); }
        public static bool GetMachineIsVirtual() { return ExeProtect.GetMachineIsVirtual(); }
        public static void LockFile(string fileName) { ExeProtect.LockFile(fileName); }
        public static void LockFile(string fileName, string MachineID) { ExeProtect.LockFile(fileName, MachineID); }
        public static ExeProtect.Validity ValidateMachine() { return ExeProtect.ValidateMachine(); }
        public static ExeProtect.Validity ValidateMachine(out string MachineID) { return ExeProtect.ValidateMachine(out MachineID); }        
        public static void UnlockFile(string fileName) { ExeProtect.UnlockFile(fileName); }        
    }

    /// <summary>
    ///     string machineID;
    ///     dkxce.ExeProtect.Validity valid = dkxce.ExeProtect.ValidateMachine(out machineID);
    ///     if((valid != dkxce.ExeProtect.Validity.Empty) && (valid != dkxce.ExeProtect.Validity.Invalid)) return;
    /// </summary>
    public class ExeProtect
    {
        public class NoHeaderException : Exception { public NoHeaderException(string text) : base(text) { } public override string ToString() { return base.ToString(); } }

        public static class Defaults
        {
            public const string Salt   = "a15122021pyT0o";      // Default Salt to Encrypt/Decrypt MachineID code in Exe File
            public const string Key    = "dkxce.ExeProtection"; // Default Key to Encrypt/Decrypt MachineID code in Exe File
            public const SysInfo Info  = SysInfo.BIOS | SysInfo.MotherBoard | SysInfo.DiskC; // Type of info for detect MachineID
        }

        public enum Validity : sbyte
        {
            Empty = 0,
            Valid = 1,
            Invalid = -1,
            BadStorage = -2,
            NoStorage = -3,
            Unused = -4
        }

        [Flags]
        public enum SysInfo : byte
        {
            Null = 0x00,
            BIOS = 0x01,
            MotherBoard = 0x02,
            CPU = 0x04,
            DiskC = 0x08,
            CurrentDisk = 0x10
        }

        private const byte CustomByteA  = 0x62; // 0x62
        private const byte CustomByteB  = 0x66; // 0x66                
        private const int  EmptySum     = 120;  // 0x3C, 0x3C // END
        private const bool EmptyCheck   = true;
        private const byte PrefixLength = 30;        
        private static byte[] MACHINE_STORED = new byte[] { 0x72, 0x65, 0x67, CustomByteA, 0x6b, CustomByteA, 0x64, 0x76, CustomByteB, 0x75, CustomByteB, 0x70, CustomByteA, 0x79, 0x74, 0x68, 0x74, 0x70, CustomByteA, 0x79, 0x6a, 0x64, 0x65, 0x2e, 0x70, CustomByteA, 0x79, 0x65, 0x3E, 0x3E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3C, 0x3C };
        private static byte[] MACHINE_PREFIX = new byte[] { 0x72, 0x65, 0x67, CustomByteA, 0x6b, CustomByteA, 0x64, 0x76, CustomByteB, 0x75, CustomByteB, 0x70, CustomByteA, 0x79, 0x74, 0x68, 0x74, 0x70, CustomByteA, 0x79, 0x6a, 0x64, 0x65, 0x2e, 0x70, CustomByteA, 0x79, 0x65, 0x3E, 0x3E };
        private static byte[] DEFAULT_SALT { get { return Encoding.ASCII.GetBytes(Defaults.Salt); } }

        /// <summary>
        ///     Validate MachineID from this file (with default key, salt & system id)
        /// </summary>
        /// <returns></returns>
        public static Validity ValidateMachine()
        {
            string MachineID;
            return ValidateMachine(Defaults.Key, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file (with default key, salt & system id)
        /// </summary>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(out string MachineID)
        {
            return ValidateMachine(Defaults.Key, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file (with default key & salt)
        /// </summary>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(SysInfo kind_of_info)
        {
            string MachineID;
            return ValidateMachine(Defaults.Key, kind_of_info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file (with default key & salt)
        /// </summary>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(SysInfo kind_of_info, out string MachineID)
        {
            return ValidateMachine(Defaults.Key, kind_of_info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file (with default salt & system id)
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password)
        {
            string MachineID;
            return ValidateMachine(key_or_password, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file (with default salt & system id)
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password, out string MachineID)
        {
            return ValidateMachine(key_or_password, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file (with default salt)
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password, SysInfo kind_of_info)
        {
            string MachineID;
            return ValidateMachine(key_or_password, kind_of_info, DEFAULT_SALT, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file (with default salt)
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password, SysInfo kind_of_info, out string MachineID)
        {
            return ValidateMachine(key_or_password, kind_of_info, DEFAULT_SALT, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password, SysInfo kind_of_info, string salt)
        {
            string MachineID;
            return ValidateMachine(key_or_password, kind_of_info, Encoding.ASCII.GetBytes(salt), out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password, SysInfo kind_of_info, string salt, out string MachineID)
        {
            return ValidateMachine(key_or_password, kind_of_info, Encoding.ASCII.GetBytes(salt), out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password, SysInfo kind_of_info, byte[] salt)
        {
            string MachineID;
            return ValidateMachine(key_or_password, kind_of_info, salt, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from this file
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateMachine(string key_or_password, SysInfo kind_of_info, byte[] salt, out string MachineID)
        {
            MachineID = SysID.GetHashFrom(
                (kind_of_info & SysInfo.BIOS) == SysInfo.BIOS, 
                (kind_of_info & SysInfo.MotherBoard) == SysInfo.MotherBoard,
                (kind_of_info & SysInfo.CPU) == SysInfo.CPU,
                (kind_of_info & SysInfo.DiskC) == SysInfo.DiskC,
                (kind_of_info & SysInfo.CurrentDisk) == SysInfo.CurrentDisk
                );

            string Hashed_MachineID = Encrypt(MachineID, key_or_password, salt);
            string Hashed_StoredID = "";
            try
            {                                
                int length_from_start = 0;
                for (int i = 0; i < MACHINE_STORED.Length; i++) if (MACHINE_STORED[i] == 0) { length_from_start = i; break; };
                if(EmptyCheck)
                {
                    int sum = 0;
                    for (int i = PrefixLength; i < MACHINE_STORED.Length; i++) sum += MACHINE_STORED[i];
                    if (sum == EmptySum) return Validity.Empty; // 0x3C, 0x3C
                };
                Hashed_StoredID = System.Text.Encoding.ASCII.GetString(MACHINE_STORED, PrefixLength, length_from_start - PrefixLength);
            }
            catch { return Validity.BadStorage; };
            
            string StoredID = "";
            try { StoredID = Decrypt(Hashed_StoredID, key_or_password, salt); } catch { };

            return MachineID == StoredID ? Validity.Valid : Validity.Invalid;
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt, key & system id)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file)
        {
            string MachineID;
            return ValidateFromFile(file, Defaults.Key, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt, key & system id)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, out string MachineID)
        {
            return ValidateFromFile(file, Defaults.Key, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt & key)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, SysInfo kind_of_info)
        {
            string MachineID;
            return ValidateFromFile(file, Defaults.Key, kind_of_info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt & key)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, SysInfo kind_of_info, out string MachineID)
        {
            return ValidateFromFile(file, Defaults.Key, kind_of_info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt & SystemID)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password)
        {
            string MachineID;
            return ValidateFromFile(file, key_or_password, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt & SystemID)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password, out string MachineID)
        {
            return ValidateFromFile(file, key_or_password, Defaults.Info, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password, SysInfo kind_of_info)
        {
            string MachineID;
            return ValidateFromFile(file, key_or_password, kind_of_info, DEFAULT_SALT, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file (with default salt)
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password, SysInfo kind_of_info, out string MachineID)
        {
            return ValidateFromFile(file, key_or_password, kind_of_info, DEFAULT_SALT, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password, SysInfo kind_of_info, string salt)
        {
            string MachineID;
            return ValidateFromFile(file, key_or_password, kind_of_info, Encoding.ASCII.GetBytes(salt), out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password, SysInfo kind_of_info, string salt, out string MachineID)
        {
            return ValidateFromFile(file, key_or_password, kind_of_info, Encoding.ASCII.GetBytes(salt), out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password, SysInfo kind_of_info, byte[] salt)
        {
            string MachineID;
            return ValidateFromFile(file, key_or_password, kind_of_info, salt, out MachineID);
        }

        /// <summary>
        ///     Validate MachineID from external file
        /// </summary>
        /// <param name="file">external filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <param name="MachineID">MachineID</param>
        /// <returns>Validation Result</returns>
        public static Validity ValidateFromFile(string file, string key_or_password, SysInfo kind_of_info, byte[] salt, out string MachineID)
        {
            MachineID = SysID.GetHashFrom(
                (kind_of_info & SysInfo.BIOS) == SysInfo.BIOS,
                (kind_of_info & SysInfo.MotherBoard) == SysInfo.MotherBoard,
                (kind_of_info & SysInfo.CPU) == SysInfo.CPU,
                (kind_of_info & SysInfo.DiskC) == SysInfo.DiskC,
                (kind_of_info & SysInfo.CurrentDisk) == SysInfo.CurrentDisk
                );

            string Hashed_MachineID = Encrypt(MachineID, key_or_password, salt);
            string Hashed_StoredID = "";
            try
            {
                byte[] FILE_STORED = GetMachineStoredFromFile(file);
                int length_from_start = 0;
                for (int i = 0; i < FILE_STORED.Length; i++) if (FILE_STORED[i] == 0) { length_from_start = i; break; };
                if (EmptyCheck)
                {
                    int sum = 0;
                    for (int i = PrefixLength; i < FILE_STORED.Length; i++) sum += FILE_STORED[i];
                    if (sum == EmptySum) return Validity.Empty; // 0x3C, 0x3C
                };
                Hashed_StoredID = System.Text.Encoding.ASCII.GetString(FILE_STORED, PrefixLength, length_from_start - PrefixLength);
                //return true;
            }
            catch (FileNotFoundException) { return Validity.NoStorage; }
            catch (NoHeaderException) { return Validity.BadStorage; }
            catch (Exception) { return Validity.BadStorage; };

            string StoredID = "";
            try { StoredID = Decrypt(Hashed_StoredID, key_or_password, salt); }
            catch { };

            return MachineID == StoredID ? Validity.Valid : Validity.Invalid;
        }

        /// <summary>
        ///     Encrypt text with key & default salt
        /// </summary>
        /// <param name="plainText">text to encrypt</param>
        /// <param name="sharedSecret">key or password</param>
        /// <returns>encrypted text</returns>
        public static string Encrypt(string plainText, string sharedSecret)
        {
            return Encrypt(plainText, sharedSecret, DEFAULT_SALT);
        }

        /// <summary>
        ///     Encrypt text with key & salt
        /// </summary>
        /// <param name="plainText">text to encrypt</param>
        /// <param name="sharedSecret">key or password</param>
        /// <param name="salt">salt</param>
        /// <returns>encrypted text</returns>
        public static string Encrypt(string plainText, string sharedSecret, string salt)
        {
            return Encrypt(plainText, sharedSecret, Encoding.ASCII.GetBytes(salt));
        }

        /// <summary>
        ///     Encrypt text with key & salt
        /// </summary>
        /// <param name="plainText">text to encrypt</param>
        /// <param name="sharedSecret">key or password</param>
        /// <param name="salt">salt</param>
        /// <returns>encrypted text</returns>
        public static string Encrypt(string plainText, string sharedSecret, byte[] salt)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        ///     Decrypt text with key & default salt
        /// </summary>
        /// <param name="cipherText">text to decrypt</param>
        /// <param name="sharedSecret">key or password</param>
        /// <returns>decrypted text</returns>
        public static string Decrypt(string cipherText, string sharedSecret)
        {
            return Decrypt(cipherText, sharedSecret, DEFAULT_SALT);
        }

        /// <summary>
        ///     Decrypt text with key & salt
        /// </summary>
        /// <param name="cipherText">text to decrypt</param>
        /// <param name="sharedSecret">key or password</param>
        /// <param name="salt">salt</param>
        /// <returns>decrypted text</returns>
        public static string Decrypt(string cipherText, string sharedSecret, string salt)
        {
            return Decrypt(cipherText, sharedSecret, Encoding.ASCII.GetBytes(salt));
        }

        /// <summary>
        ///     Decrypt text with key & salt
        /// </summary>
        /// <param name="cipherText">text to decrypt</param>
        /// <param name="sharedSecret">key or password</param>
        /// <param name="salt">salt</param>
        /// <returns>decrypted text</returns>
        public static string Decrypt(string cipherText, string sharedSecret, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            catch
            {
                plaintext = "DECRYPT_ERROR";
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        /// <summary>
        ///     Get local MachineID (with Default SysInfo)
        /// </summary>
        /// <returns>MachineID</returns>
        public static string GetMachineID()
        {
            return GetMachineID(Defaults.Info);
        }

        /// <summary>
        ///     Get local MachineID
        /// </summary>
        /// <param name="kind_of_info">kind of info for system id</param>
        /// <returns>MachineID</returns>
        public static string GetMachineID(SysInfo kind_of_info)
        {
            return SysID.GetHashFrom(
                (kind_of_info & SysInfo.BIOS) == SysInfo.BIOS,
                (kind_of_info & SysInfo.MotherBoard) == SysInfo.MotherBoard,
                (kind_of_info & SysInfo.CPU) == SysInfo.CPU,
                (kind_of_info & SysInfo.DiskC) == SysInfo.DiskC,
                (kind_of_info & SysInfo.CurrentDisk) == SysInfo.CurrentDisk
                );
        }

        /// <summary>
        ///     Get Machine is Virtual
        /// </summary>
        /// <returns></returns>
        public static bool GetMachineIsVirtual()
        {
            return SysID.IsVirtual;
        }

        /// <summary>
        ///     Get external file locked MachineID
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <returns>MachineID</returns>
        public static string GetMachineIDFromFile(string fileName, string key_or_password)
        {
            return GetMachineIDFromFile(fileName, key_or_password, DEFAULT_SALT);
        }

        /// <summary>
        ///     Get external file locked MachineID
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <returns>MachineID</returns>
        public static string GetMachineIDFromFile(string fileName, string key_or_password, string salt)
        {
            return GetMachineIDFromFile(fileName, key_or_password, Encoding.ASCII.GetBytes(salt));
        }

        /// <summary>
        ///     Get external file locked MachineID
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="salt">salt to decrypt MachineID</param>
        /// <returns>MachineID</returns>
        public static string GetMachineIDFromFile(string fileName, string key_or_password, byte[] salt)
        {
            string Hashed_StoredID = "";
            try
            {
                byte[] FILE_STORED = GetMachineStoredFromFile(fileName);
                int length_from_start = 0;
                for (int i = 0; i < FILE_STORED.Length; i++) if (FILE_STORED[i] == 0) { length_from_start = i; break; };
                if(EmptyCheck)
                {
                    int sum = 0;
                    for (int i = PrefixLength; i < FILE_STORED.Length; i++) sum += FILE_STORED[i];
                    if (sum == EmptySum) return "EMPTY"; // 0x3C, 0x3C
                };
                Hashed_StoredID = System.Text.Encoding.ASCII.GetString(FILE_STORED, PrefixLength, length_from_start - PrefixLength);
            }
            catch (FileNotFoundException) { return "ERROR, NO FILE"; }
            catch (NoHeaderException)  { return "ERROR, NO HEADER"; }
            catch (Exception) { return "ERROR"; };

            string StoredID = "";
            try { StoredID = Decrypt(Hashed_StoredID, key_or_password, salt); }
            catch { };

            return StoredID;
        }

        /// <summary>
        ///     Get this file locked MachineID
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <returns>MachineID</returns>
        public static string GetMachineIDThisFile(string key_or_password)
        {
            return GetMachineIDThisFile(key_or_password, DEFAULT_SALT);
        }

        /// <summary>
        ///     Get this file locked MachineID
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="salt">sale to decrypt MachineID</param>
        /// <returns>MachineID</returns>
        public static string GetMachineIDThisFile(string key_or_password, string salt)
        {
            return GetMachineIDThisFile(key_or_password, Encoding.ASCII.GetBytes(salt));
        }

        /// <summary>
        ///     Get this file locked MachineID
        /// </summary>
        /// <param name="key_or_password">key to decrypt MachineID</param>
        /// <param name="salt">sale to decrypt MachineID</param>
        /// <returns>MachineID</returns>
        public static string GetMachineIDThisFile(string key_or_password, byte[] salt)
        {
            string Hashed_StoredID = "";
            try
            {
                int length_from_start = 0;
                for (int i = 0; i < MACHINE_STORED.Length; i++) if (MACHINE_STORED[i] == 0) { length_from_start = i; break; };
                if(EmptyCheck)
                {
                    int sum = 0;
                    for (int i = PrefixLength; i < MACHINE_STORED.Length; i++) sum += MACHINE_STORED[i];
                    if (sum == EmptySum) return "EMPTY"; // 0x3C, 0x3C
                };
                Hashed_StoredID = System.Text.Encoding.ASCII.GetString(MACHINE_STORED, PrefixLength, length_from_start - PrefixLength);
            }
            catch (Exception) { return "ERROR"; };

            string StoredID = "";
            try { StoredID = Decrypt(Hashed_StoredID, key_or_password, salt); }
            catch { };

            return StoredID;
        }

        /// <summary>
        ///     Unlock external file
        /// </summary>
        /// <param name="fileName">filename</param>
        public static void UnlockFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            int i = 0;
            int B = fs.ReadByte();
            while (i < MACHINE_PREFIX.Length)
            {
                if (B != MACHINE_PREFIX[i++]) i = 0;
                B = fs.ReadByte();
                if (B < 0) { fs.Close(); throw new NoHeaderException("File doesn't contains MACHINE_STORED data"); };
            };
            fs.Position--;
            byte[] ba = new byte[512];
            fs.Write(ba, 0, ba.Length);
            fs.Close();
        }

        /// <summary>
        ///     Lock external file to this MachineID (with default salt and key)
        /// </summary>
        /// <param name="fileName"></param>
        public static void LockFile(string fileName)
        {
            LockFile(fileName, GetMachineID(), Defaults.Key, DEFAULT_SALT);
        }

        /// <summary>
        ///     Lock external file to MachineID (with default salt and key)
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="MachineID">MachineID</param>
        public static void LockFile(string fileName, string MachineID)
        {
            LockFile(fileName, MachineID, Defaults.Key, DEFAULT_SALT);
        }

        /// <summary>
        ///     Lock external file to MachineID (with default salt)
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="MachineID">MachineID</param>
        /// <param name="key_or_password">key to encrypt MachineID</param>        
        public static void LockFile(string fileName, string MachineID, string key_or_password)
        {
            LockFile(fileName, MachineID, String.IsNullOrEmpty(key_or_password) ? Defaults.Key : key_or_password, DEFAULT_SALT);
        }

        /// <summary>
        ///     Lock external file to MachineID
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="MachineID">MachineID</param>
        /// <param name="key_or_password">key to encrypt MachineID</param>        
        /// <param name="salt">salt to encrypt MachineID</param>
        public static void LockFile(string fileName, string MachineID, string key_or_password, string salt)
        {
            LockFile(fileName, MachineID, String.IsNullOrEmpty(key_or_password) ? Defaults.Key : key_or_password, String.IsNullOrEmpty(salt) ? DEFAULT_SALT : Encoding.ASCII.GetBytes(salt));
        }

        /// <summary>
        ///     Lock external file to MachineID
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="MachineID">MachineID</param>
        /// <param name="key_or_password">key to encrypt MachineID</param>        
        /// <param name="salt">salt to encrypt MachineID</param>
        public static void LockFile(string fileName, string MachineID, string key_or_password, byte[] salt)
        {
            string HASH = Encrypt(MachineID, String.IsNullOrEmpty(key_or_password) ? Defaults.Key : key_or_password, salt == null ? DEFAULT_SALT : salt);
            LockFile2Hash(fileName, HASH);
        }
        
        /// <summary>
        ///     Write Hashed Text (MachineID) to MACHINE_STORED in external file 
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <param name="hash">hashed MachineID</param>
        private static void LockFile2Hash(string fileName, string hash)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            int i = 0;
            int B = fs.ReadByte();
            while (i < MACHINE_PREFIX.Length)
            {
                if (B != MACHINE_PREFIX[i++]) i = 0;
                B = fs.ReadByte();
                if (B < 0) { fs.Close(); throw new NoHeaderException("File doesn't contains MACHINE_STORED data"); };
            };
            fs.Position--;
            byte[] ba = System.Text.Encoding.ASCII.GetBytes(hash);
            fs.Write(ba, 0, ba.Length);
            ba = new byte[512 - ba.Length];
            fs.Write(ba, 0, ba.Length);
            fs.Close();
        }
        
        /// <summary>
        ///     Get Array like MACHINE_STORED data from external file
        /// </summary>
        /// <param name="fileName">file Name</param>
        /// <returns>MACHINE_STORED</returns>
        private static byte[] GetMachineStoredFromFile(string fileName)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException("File not found", fileName);
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int i = 0;
            int B = fs.ReadByte();
            while (i < MACHINE_PREFIX.Length)
            {
                if (B != MACHINE_PREFIX[i++]) i = 0;
                B = fs.ReadByte();
                if (B < 0) { fs.Close(); throw new NoHeaderException("File doesn't contains MACHINE_STORED data"); };
            };
            fs.Position = fs.Position - 1 - MACHINE_PREFIX.Length;
            byte[] ba = new byte[544];
            fs.Read(ba, 0, ba.Length);
            fs.Close();
            return ba;
        }        

        private static byte[] ReadByteArray(System.IO.Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
                throw new SystemException("Stream did not contain properly formatted byte array");

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
                throw new SystemException("Did not read byte array properly");

            return buffer;
        }
    }

    /// <summary>
    ///     System Information
    /// </summary>
    public static class SysID
    {
        public delegate void OnInfoData(string name, object value);
        public static OnInfoData onInfoData;

        public static string BIOS
        {
            get
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                ManagementObjectCollection moc = mos.Get();
                string motherBoard = "";
                foreach (ManagementObject mo in moc)
                {
                    if (onInfoData != null)
                    {
                        onInfoData("", "BIOS");
                        foreach (PropertyData pd in mo.Properties)
                            if (!String.IsNullOrEmpty(pd.Name))
                                onInfoData(pd.Name, pd.Value);                        
                    };
                    motherBoard = (string)mo["SerialNumber"];
                }
                return motherBoard;
            }
        }

        public static string MotherBoard
        {
            get
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                ManagementObjectCollection moc = mos.Get();
                string motherBoard = "";
                foreach (ManagementObject mo in moc)
                {
                    if (onInfoData != null)
                    {
                        onInfoData("", "MotherBoard");
                        foreach (PropertyData pd in mo.Properties)
                            if(!String.IsNullOrEmpty(pd.Name))
                                onInfoData(pd.Name, pd.Value);                        
                    };
                    motherBoard = (string)mo["SerialNumber"];
                };
                return motherBoard;
            }
        }

        public static string Processor
        {
            get
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_processor");
                ManagementObjectCollection moc = mos.Get();
                string motherBoard = "";
                foreach (ManagementObject mo in moc)
                {
                    if (onInfoData != null)
                    {
                        onInfoData("", "Processor");
                        foreach (PropertyData pd in mo.Properties)
                            if (!String.IsNullOrEmpty(pd.Name))
                                onInfoData(pd.Name, pd.Value);                        
                    };
                    motherBoard = (string)mo["ProcessorID"];
                };
                return motherBoard;
            }
        }

        public static string DiskC
        {
            get
            {
                ManagementObject dsk = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
                dsk.Get();
                if (onInfoData != null)
                {
                    onInfoData("", "DiskC");
                    foreach (PropertyData pd in dsk.Properties)
                        if (!String.IsNullOrEmpty(pd.Name))
                            onInfoData(pd.Name, pd.Value);                    
                };                
                return dsk["VolumeSerialNumber"].ToString();
            }
        }

        public static string CurrentDisk
        {
            get
            {
                ManagementObject dsk = new ManagementObject("win32_logicaldisk.deviceid=\"" + AppDomain.CurrentDomain.BaseDirectory.Substring(0, 1) + ":\"");
                dsk.Get();
                if (onInfoData != null)
                {
                    onInfoData("", "CurrentDisk");
                    foreach (PropertyData pd in dsk.Properties)
                        if (!String.IsNullOrEmpty(pd.Name))
                            onInfoData(pd.Name, pd.Value);                    
                };
                return dsk["VolumeSerialNumber"].ToString();
            }
        }

        public static bool IsVirtual
        {
            get
            {
                {
                    ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_processor");
                    ManagementObjectCollection moc = mos.Get();
                    string val = "";
                    foreach (ManagementObject mo in moc)
                        val = (string)mo["Manufacturer"];
                    if (val.Contains("VBoxVBoxVBox")) return true;
                    if (val.Contains("VMwareVMware")) return true;
                    if (val.Contains("prl hyperv")) return true;
                };
                {
                    ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                    ManagementObjectCollection moc = mos.Get();
                    string val = "";
                    foreach (ManagementObject mo in moc)
                        val = (string)mo["Manufacturer"];
                    if (val.Contains("Microsoft Corporation")) return true;
                };
                {
                    ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                    ManagementObjectCollection moc = mos.Get();
                    string val = "";
                    foreach (ManagementObject mo in moc)
                        val = (string)mo["PNPDeviceID"];
                    if (val.Contains("VBOX_HARDDISK")) return true;
                    if (val.Contains("VEN_VMWARE")) return true;
                };
                return false;
            }
        }

        public static string GetHashFrom(bool wBIOS, bool wMotherBoard, bool wCPU, bool wDiskC, bool wCurrDisk)
        {
            string code = "";
            try
            {
                if (wBIOS) code += "/" + BIOS;
            }
            catch { };
            try
            {
                if (wMotherBoard) code += "/" + MotherBoard;
            }
            catch { };
            try
            {
                if (wCPU) code += "/" + Processor;
            }
            catch { };
            try
            {
                if (wDiskC) code += "/" + DiskC;
            }
            catch { };
            try
            {
                if (wCurrDisk) code += "/" + CurrentDisk;
            }
            catch { };

            //int codeType = (wBIOS ? 1 : 0) + (wMotherBoard ? 2 : 0) + (wCPU ? 4 : 0) + (wDiskC ? 8 : 0) + (wCurrDisk ? 0x10 : 0);

            return GetHash(code);
        }

        public static string DefaultSystemHash
        {
            get
            {
                return GetHashFrom(true, true, false, true, false);
            }
        }

        private static string GetHash(string s)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] bt = enc.GetBytes(s);
            return GetHexString(sec.ComputeHash(bt));
        }

        private static string GetHexString(byte[] bt)
        {
            string s = string.Empty;
            for (int i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int n, n1, n2;
                n = (int)b;
                n1 = n & 15;
                n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + (int)'A')).ToString();
                else
                    s += n2.ToString();
                if (n1 > 9)
                    s += ((char)(n1 - 10 + (int)'A')).ToString();
                else
                    s += n1.ToString();
                if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
            }
            return s;
        }               
    }

    public static class FindMethods
    {
        public static int FindIn(byte[] where2find, int offset, byte[] what2find)
        {
            if ((where2find == null) || (what2find == null) || (what2find.Length == 0) || (what2find.Length == 0)) return -1;
            int i = 0, n = offset, B = where2find[n++];
            while (i < what2find.Length)
            {
                if (B != what2find[i++]) i = 0;
                B = where2find[n++];
                if (n == where2find.Length) return -1;
            };
            return n - i - 1;
        }

        public static int FindIn(byte[] where2find, byte[] what2find)
        {
            return FindIn(where2find, 0, what2find);
        }

        public static int FindIn(string where2find, int offset, string what2find, System.Text.Encoding encoding)
        {
            if (String.IsNullOrEmpty(where2find)) return -1;
            if (String.IsNullOrEmpty(what2find)) return -1;
            return FindIn(encoding.GetBytes(where2find), offset, encoding.GetBytes(what2find));
        }

        public static int FindIn(string where2find, string what2find, System.Text.Encoding encoding)
        {
            return FindIn(where2find, 0, what2find, encoding);
        }

        public static long FindIn(Stream stream, string what2find, System.Text.Encoding encoding)
        {
            if (String.IsNullOrEmpty(what2find)) return -1;
            return FindIn(stream, encoding.GetBytes(what2find));
        }

        public static long FindIn(Stream stream, long offset, string what2find, System.Text.Encoding encoding)
        {
            if (String.IsNullOrEmpty(what2find)) return -1;
            return FindIn(stream, offset, encoding.GetBytes(what2find));
        }

        public static long FindIn(Stream stream, byte[] what2find)
        {
            return FindIn(stream, 0, what2find);
        }

        public static long FindIn(Stream stream, long offset, byte[] what2find)
        {
            if ((stream == null) || (stream.Length == 0)) return -1;
            if ((what2find == null) || (what2find.Length == 0)) return -1;
            stream.Position = offset;
            int i = 0, B = stream.ReadByte();
            while (i < what2find.Length)
            {
                if (B != what2find[i++]) i = 0;
                B = stream.ReadByte();
                if (B < 0) { return -1; };
            };
            return stream.Position - i - 1;
        }
    }
}

/*
        // SAMPLE
 
        [STAThread]
        static void Main()
        {
            string machineID;
            dkxce.ExeProtect.Validity valid = dkxce.ExeProtect.ValidateMachine(out machineID);
            // allow empty
            if((valid != dkxce.ExeProtect.Validity.Empty) && (valid != dkxce.ExeProtect.Validity.Invalid)) return;
            // or not
            if(valid != dkxce.ExeProtect.Validity.Invalid) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form());
        }
*/
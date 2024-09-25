using System;
using System.Text.RegularExpressions;

namespace VTIWindowsControlLibrary.Classes.Data
{
    /// <summary>
    /// Holds database schema revision information.
    /// </summary>
    public sealed class SchemaRevision
    {
        #region Fields (3) 

        #region Private Fields (3) 

        private readonly int _Major;
        private readonly int _Minor;
        private readonly int _Release;

        #endregion Private Fields 

        #endregion Fields 

        #region Constructors (1) 

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaRevision">SchemaRevision</see> class
        /// </summary>
        /// <param name="Major">Major revision</param>
        /// <param name="Minor">Minor revision</param>
        /// <param name="Release">Release</param>
        public SchemaRevision(int Major, int Minor, int Release)
        {
            if (Major < 1)
                throw new ArgumentOutOfRangeException("major", "Major version must be positive value.");
            if (Minor < 0)
                throw new ArgumentOutOfRangeException("minor", "Minor version must be non-negative value.");
            if (Release < 0)
                throw new ArgumentOutOfRangeException("release", "Release number must be non-negative value.");

            this._Major = Major;
            this._Minor = Minor;
            this._Release = Release;
        }

        #endregion Constructors 

        #region Properties (3) 

        /// <summary>
        /// Major revision number.
        /// </summary>
        public int Major
        {
            get { return _Major; }
        }

        /// <summary>
        /// Minor revision number.
        /// </summary>
        public int Minor
        {
            get { return _Minor; }
        }

        /// <summary>
        /// Release number.
        /// </summary>
        public int Release
        {
            get { return _Release; }
        }

        #endregion Properties 

        #region Methods (7) 

        #region Public Methods (7) 

        /// <summary>
        /// Compares two <see cref="SchemaRevision">SchemaRevision</see> objects to
        /// see if they are inequal, by comparing their
        /// <see cref="Major">Major Revision</see>,
        /// <see cref="Minor">Minor Revision</see>, and
        /// <see cref="Release">Release Number</see>.
        /// </summary>
        /// <param name="left"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <param name="right"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <returns>True if the two <see cref="SchemaRevision">SchemaRevision</see> objects are inequal.</returns>
        public static bool operator !=(SchemaRevision left, SchemaRevision right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares two <see cref="SchemaRevision">SchemaRevision</see> objects to
        /// see if they are equal, by comparing their
        /// <see cref="Major">Major Revision</see>,
        /// <see cref="Minor">Minor Revision</see>, and
        /// <see cref="Release">Release Number</see>.
        /// </summary>
        /// <param name="left"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <param name="right"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <returns>True if the two <see cref="SchemaRevision">SchemaRevision</see> objects are equal.</returns>
        public static bool operator ==(SchemaRevision left, SchemaRevision right)
        {
            object n = null;

            //  Both are null.
            if (object.ReferenceEquals(n, left) && object.ReferenceEquals(n, right))
                return true;

            //  Neither are null and fields are equal.
            return (!object.ReferenceEquals(n, left) && !object.ReferenceEquals(n, right) &&
                (left.Major == right.Major && left.Minor == right.Minor && left.Release == right.Release));
        }

        /// <summary>
        /// Clones schema revision information.
        /// </summary>
        public SchemaRevision Clone()
        {
            return new SchemaRevision(Major, Minor, Release);
        }

        /// <summary>
        /// Determines whether the specified
        /// <see cref="System.Object">System.Object</see> is equal to this
        /// <see cref="SchemaRevision">SchemaRevision</see>
        /// </summary>
        /// <param name="obj">The <see cref="System.Object">System.Object</see> to compare with this
        /// <see cref="SchemaRevision">SchemaRevision</see></param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SchemaRevision)
                return (obj as SchemaRevision) == this;

            return false;
        }

        /// <summary>
        /// Creates new instance of the schema revision object from file name.
        /// </summary>
        /// <param name="fileName">File name. Could be either full path or just a file name.
        /// File name must be in update-MAJ.MIN.REL.ddl format.</param>
        public static SchemaRevision FromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("FileName");

            int major = 0, minor = 0, release = 0;

            try
            {
                //  Parse file name.
                Regex fileNameRegex =
                    new Regex(@"update\-(?<major>\d{3})\.(?<minor>\d{3})\.(?<release>\d{3})\.ddl$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match revisionMatch = fileNameRegex.Match(fileName);
                if (revisionMatch.Success)
                {
                    major = Convert.ToInt32(revisionMatch.Groups["major"].Value);
                    minor = Convert.ToInt32(revisionMatch.Groups["minor"].Value);
                    release = Convert.ToInt32(revisionMatch.Groups["release"].Value);
                    if (release < 1)
                        throw new ArgumentOutOfRangeException("release", "When constructing from the file name, Release must be positive value.");
                    return new SchemaRevision(major, minor, release);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("File name doesn't contain revision information.", ex);
            }

            throw new ArgumentException("File name doesn't contain revision information.");
        }

        /// <summary>
        /// Serves as a hash function for the SchemaRevision type
        /// </summary>
        /// <returns>Unique hash code for any SchemaRevision object</returns>
        public override int GetHashCode()
        {
            return Major * 1000000 + Minor * 1000 + Release;
        }

        /// <summary>
        /// Converts the SchemaRevision to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", Major, Minor, Release);
        }

        #endregion Public Methods 

        #endregion Methods 

        /// <summary>
        /// Compares two <see cref="SchemaRevision">SchemaRevision</see> objects to
        /// see if the left object is less than the right object, by comparing their
        /// <see cref="Major">Major Revision</see>,
        /// <see cref="Minor">Minor Revision</see>, and
        /// <see cref="Release">Release Number</see>.
        /// </summary>
        /// <param name="left"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <param name="right"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <returns>True if the left object is less than the right object.</returns>
        public static bool operator <(SchemaRevision left, SchemaRevision right)
        {
            return left.Major < right.Major || (left.Major == right.Major && left.Minor < right.Minor) ||
                (left.Major == right.Major && left.Minor == right.Minor && left.Release < right.Release);
        }

        /// <summary>
        /// Compares two <see cref="SchemaRevision">SchemaRevision</see> objects to
        /// see if the left object is greater than the right object, by comparing their
        /// <see cref="Major">Major Revision</see>,
        /// <see cref="Minor">Minor Revision</see>, and
        /// <see cref="Release">Release Number</see>.
        /// </summary>
        /// <param name="left"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <param name="right"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <returns>True if the left object is greater than the right object.</returns>
        public static bool operator >(SchemaRevision left, SchemaRevision right)
        {
            return left.Major > right.Major || (left.Major == right.Major && left.Minor > right.Minor) ||
                (left.Major == right.Major && left.Minor == right.Minor && left.Release > right.Release);
        }

        /// <summary>
        /// Compares two <see cref="SchemaRevision">SchemaRevision</see> objects to
        /// see if the left object is less than or equal to the right object, by comparing their
        /// <see cref="Major">Major Revision</see>,
        /// <see cref="Minor">Minor Revision</see>, and
        /// <see cref="Release">Release Number</see>.
        /// </summary>
        /// <param name="left"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <param name="right"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <returns>True if the left object is less than or equal to the right object.</returns>
        public static bool operator <=(SchemaRevision left, SchemaRevision right)
        {
            return (left < right || left == right);
        }

        /// <summary>
        /// Compares two <see cref="SchemaRevision">SchemaRevision</see> objects to
        /// see if the left object is greater than or equal to the right object, by comparing their
        /// <see cref="Major">Major Revision</see>,
        /// <see cref="Minor">Minor Revision</see>, and
        /// <see cref="Release">Release Number</see>.
        /// </summary>
        /// <param name="left"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <param name="right"><see cref="SchemaRevision">SchemaRevision</see> object</param>
        /// <returns>True if the left object is greater than or equal to the right object.</returns>
        public static bool operator >=(SchemaRevision left, SchemaRevision right)
        {
            return (left > right || left == right);
        }
    }
}
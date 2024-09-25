using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.Configuration.Interfaces;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Classes
{
    /// <summary>
    /// Base class from which all Model settings must be derived
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [XmlRoot("ModelSettings")]
    public class ModelSettingsBase : EditCycleApplicationSettingsBase
    {
        #region Fields (1)

        #region Public Fields (1)

        /// <summary>
        /// Name of the model
        /// </summary>
        public string Name = "Default";

        #endregion Public Fields

        #endregion Fields

        #region Delegates and Events (1)

        #region Events (1)

        /// <summary>
        /// Occurs when model settings are loaded from the database
        /// </summary>
        public event EventHandler Loaded;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (4)

        #region Public Methods (3)

        /// <summary>
        /// Loads a model from the database
        /// </summary>
        /// <param name="modelNo">Name of the model to be loaded</param>
        /// <returns>True if successful</returns>
        public bool Load(string modelNo)
        {
            bool dbState = VtiLib.MuteParamChangeLog;
            VtiLib.MuteParamChangeLog = true;
            Type type;
            String actualModelNo;

            try
            { 
                type = this.GetType();
				if(VtiLib.UseRemoteModelDB)
				{
                    using(VTIWindowsControlLibrary.Data.VtiDataContext2.VtiDataContext2 db = new VTIWindowsControlLibrary.Data.VtiDataContext2.VtiDataContext2(VtiLib.ConnectionString3))
                    {
                        // Use cross-ref model number if one is available
                        if(db.ModelXRefs.Any(xr => xr.ScannedChars == modelNo))
                            actualModelNo = db.ModelXRefs.First(xr => xr.ScannedChars == modelNo).ModelNo;
                        else
                            actualModelNo = modelNo;

                        List<VTIWindowsControlLibrary.Data.VtiDataContext2.ModelParameter> storedParameters = db.Models.Single(m => m.ModelNo == actualModelNo).ModelParameters.Where(m => m.SystemType == VtiLib.ModelDBSystemType).ToList();

                        if(storedParameters.Count > 0)
                        {
                            foreach(VTIWindowsControlLibrary.Data.VtiDataContext2.ModelParameter param in storedParameters)
                            {
                                try
                                {
                                    if(this[param.ParameterID] != null)
                                    {
									    this[param.ParameterID].ProcessValueString = param.ProcessValue;
									    //this[param.ParameterID].MaxValue = param.MaxValue;
									    //this[param.ParameterID].MinValue = param.MinValue;
									    //this[param.ParameterID].DisplayName = param.DisplayName;
									    //this[param.ParameterID].ToolTip = param.ToolTip;
									    //this[param.ParameterID].Units = param.Units;
									    //this[param.ParameterID].ParameterType = param.ParameterType;
								    }
                                    
							    }
                                catch(Exception e2)
                                {
                                    VtiEvent.Log.WriteWarning(
                                        string.Format("Error loading model parameter " + param.ParameterID),
                                        VtiEventCatType.Database,
                                        e2.ToString());
                                }
                            }
                        }
                        else
                        {
                            try 
                            { 
                                ModelSettingsBase defaultModel = (ModelSettingsBase)VtiLib.Config.GetType().GetProperty("DefaultModel").GetValue(null, null);
								foreach(SettingsProperty param in this.Properties)
								{
									db.Models.Single(m => m.ModelNo == actualModelNo).ModelParameters.Add(new VTIWindowsControlLibrary.Data.VtiDataContext2.ModelParameter
									{
										ParameterID = param.Name,
										ProcessValue = defaultModel[param.Name].ProcessValueString,
										LastModifiedBy = VtiLib.Config._OpID,
										LastModified = DateTime.Now,
										SystemType = VtiLib.ModelDBSystemType
									});
									this[param.Name].ProcessValueString = defaultModel[param.Name].ProcessValueString;
								}
                                db.SubmitChanges();
							}
							catch(Exception e2)
							{
								VtiEvent.Log.WriteWarning(
									string.Format("Error loading model " + actualModelNo),
									VtiEventCatType.Database,
									e2.ToString());
							}
							

						}

					}
                }
                else
                {
					using(VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString))
					{
						// Use cross-ref model number if one is available
						if(db.ModelXRefs.Any(xr => xr.ScannedChars == modelNo))
							actualModelNo = db.ModelXRefs.First(xr => xr.ScannedChars == modelNo).ModelNo;
						else
							actualModelNo = modelNo;

						foreach(ModelParameter param in db.Models.Single(m => m.ModelNo == actualModelNo).ModelParameters)
						{
							try
							{
								if(this[param.ParameterID] != null)
									this[param.ParameterID].ProcessValueString = param.ProcessValue;
							}
							catch(Exception e2)
							{
								VtiEvent.Log.WriteWarning(
									string.Format("Error loading model parameter " + param.ParameterID),
									VtiEventCatType.Database,
									e2.ToString());
							}
						}
					}

				}
				this.Name = actualModelNo;

				OnLoaded();

				VtiLib.MuteParamChangeLog = dbState;
                return true;
			}
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(
                    string.Format(VtiLibLocalization.ErrorLoadingModel, modelNo),
                    VtiEventCatType.Database,
                    e.ToString());

                VtiLib.MuteParamChangeLog = dbState;
                return false;
			}
        }

        /// <summary>
        /// Loads the settings for the current model from the specified model.
        /// </summary>
        /// <param name="model">Model from which to load the settings.</param>
        public void LoadFrom(ModelSettingsBase model)
        {
			bool dbState = VtiLib.MuteParamChangeLog;
			VtiLib.MuteParamChangeLog = true;
			foreach (SettingsProperty param in this.Properties)
            {
                this[param.Name].ProcessValueString = model[param.Name].ProcessValueString;
            }
            this.Name = model.Name;

            VtiLib.MuteParamChangeLog = dbState;
            OnLoaded();
		}

        /// <summary>
        /// Saves a model to the database
        /// </summary>
        public override void Save()
        {
            if (this.Name == "Default")
            {
                base.Save();
            }
            else
            {
				
                IEditCycleParameter editCycleParameter;

                if(VtiLib.UseRemoteModelDB)
                {
					VTIWindowsControlLibrary.Data.VtiDataContext2.Model model;
					VTIWindowsControlLibrary.Data.VtiDataContext2.ModelParameter modelParameter;

					using(VTIWindowsControlLibrary.Data.VtiDataContext2.VtiDataContext2 db = new VTIWindowsControlLibrary.Data.VtiDataContext2.VtiDataContext2(VtiLib.ConnectionString3))
					{
						using(TransactionScope ts = new TransactionScope())
						{
							model = db.Models.Single(m => m.ModelNo == this.Name);

							foreach(SettingsProperty param in this.Properties)
							{
								modelParameter = model.ModelParameters.Single(p => p.ParameterID == param.Name && p.SystemType == VtiLib.ModelDBSystemType);
								editCycleParameter = this[param.Name];
								try
								{
									if(editCycleParameter.ProcessValueString != editCycleParameter.NewValueString)
									{
										VtiEvent.Log.WriteWarning(
											string.Format(VtiLibLocalization.ModelParameterChanged,
												param.Name, this.Name, editCycleParameter.ProcessValueString, editCycleParameter.NewValueString),
											VtiEventCatType.Parameter_Update);
									}
									modelParameter.ProcessValue = this[param.Name].ProcessValueString;
								}
								catch(Exception e)
								{
									VtiEvent.Log.WriteError(
										string.Format(VtiLibLocalization.ModelParameterChanged,
											param.Name, this.Name, editCycleParameter.ProcessValueString, "null"),
										VtiEventCatType.Parameter_Update);
								}
							}

							db.SubmitChanges();
							ts.Complete();
						}
					}
				}
                else
                {
					Model model;
					ModelParameter modelParameter;

					using (VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString))
                    {
                        using (TransactionScope ts = new TransactionScope())
                        {
                            model = db.Models.Single(m => m.ModelNo == this.Name);

                            foreach (SettingsProperty param in this.Properties)
                            {
                                modelParameter = model.ModelParameters.Single(p => p.ParameterID == param.Name);
                                editCycleParameter = this[param.Name];
                                try
                                {
                                    if (editCycleParameter.ProcessValueString != editCycleParameter.NewValueString)
                                    {
                                        VtiEvent.Log.WriteWarning(
                                            string.Format(VtiLibLocalization.ModelParameterChanged,
                                                param.Name, this.Name, editCycleParameter.ProcessValueString, editCycleParameter.NewValueString),
                                            VtiEventCatType.Parameter_Update);
                                    }
                                    modelParameter.ProcessValue = this[param.Name].ProcessValueString;
                                }
                                catch (Exception e)
                                {
                                    VtiEvent.Log.WriteError(
                                        string.Format(VtiLibLocalization.ModelParameterChanged,
                                            param.Name, this.Name, editCycleParameter.ProcessValueString, "null"),
                                        VtiEventCatType.Parameter_Update);
                                }
                            }

                            db.SubmitChanges();
                            ts.Complete();
                        }
                    }

                
                }
            }
        }

        #endregion Public Methods
        #region Protected Methods (1)

        /// <summary>
        /// Invokes the Loaded event handler
        /// </summary>
        protected virtual void OnLoaded()
        {
            EventHandler handler = Loaded;
            if (handler != null)
                handler(this, null);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}
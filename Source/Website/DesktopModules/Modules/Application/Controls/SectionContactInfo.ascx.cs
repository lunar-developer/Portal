using System;
using System.Web.UI.WebControls;
using Modules.Application.Global;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionContactInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }

        private void BindData()
        {
            BindCountryData(ctrlHomeCountry);
            BindStateData(ctrlHomeState, GetEmptyItem());
            ctrlHomeCity.Items.Add(GetEmptyItem());

            BindCountryData(ctrlAlternative01Country, GetEmptyItem());
            ctrlAlternative01State.Items.Add(GetEmptyItem());
            ctrlAlternative01City.Items.Add(GetEmptyItem());

            BindCountryData(ctrlAlternative02Country, GetEmptyItem());
            ctrlAlternative02State.Items.Add(GetEmptyItem());
            ctrlAlternative02City.Items.Add(GetEmptyItem());
        }

        protected void ProcessOnSelectCountry(object sender, EventArgs e)
        {
            DropDownList dropDownList = sender as DropDownList;
            if (dropDownList == null)
            {
                return;
            }

            DropDownList dropDownListState;
            DropDownList dropDownListCity;
            switch (dropDownList.ID)
            {
                case "ctrlAlternative01Country":
                    dropDownListState = ctrlAlternative01State;
                    dropDownListCity = ctrlAlternative01City;
                    break;

                case "ctrlAlternative02Country":
                    dropDownListState = ctrlAlternative02State;
                    dropDownListCity = ctrlAlternative02City;
                    break;

                default:
                    dropDownListState = ctrlHomeState;
                    dropDownListCity = ctrlHomeCity;
                    break;
            }

            BindEmptyItem(dropDownListCity);
            if (dropDownList.SelectedValue != "VN")
            {
                BindEmptyItem(dropDownListState);
            }
            else
            {
                BindStateData(dropDownListState, GetEmptyItem());
            }
        }

        protected void ProcessOnSelectState(object sender, EventArgs e)
        {
            DropDownList dropDownList = sender as DropDownList;
            if (dropDownList == null)
            {
                return;
            }

            string value = dropDownList.SelectedValue;
            switch (dropDownList.ID)
            {
                case "ctrlAlternative01State":
                    dropDownList = ctrlAlternative01City;
                    break;

                case "ctrlAlternative02State":
                    dropDownList = ctrlAlternative02City;
                    break;

                default:
                    dropDownList = ctrlHomeCity;
                    break;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                BindEmptyItem(dropDownList);
            }
            else
            {
                BindCityData(dropDownList, value, GetEmptyItem());
            }
        }

        private void BindEmptyItem(ListControl dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Add(GetEmptyItem());
        }

        #region PUBLIC PROPERTY

        public TextBox ControlHomeAddress01 => ctrlHomeAddress01;
        public TextBox ControlHomeAddress02 => ctrlHomeAddress02;
        public TextBox ControlHomeAddress03 => ctrlHomeAddress03;
        public DropDownList ControlHomeCountry => ctrlHomeCountry;
        public DropDownList ControlHomeState => ctrlHomeState;
        public DropDownList ControlHomeCity => ctrlHomeCity;
        public TextBox ControlHomePhone01 => ctrlHomePhone01;
        public TextBox ControlHomeRemark => ctrlHomeRemark;


        public TextBox ControlAlternative01Address01 => ctrlAlternative01Address01;
        public TextBox ControlAlternative01Address02 => ctrlAlternative01Address02;
        public TextBox ControlAlternative01Address03 => ctrlAlternative01Address03;
        public DropDownList ControlAlternative01Country => ctrlAlternative01Country;
        public DropDownList ControlAlternative01State => ctrlAlternative01State;
        public DropDownList ControlAlternative01City => ctrlAlternative01City;
        public TextBox ControlAlternative01Phone01 => ctrlAlternative01Phone01;
        public TextBox ControlAlternative01Remark => ctrlAlternative01Remark;


        public TextBox ControlAlternative02Address01 => ctrlAlternative02Address01;
        public TextBox ControlAlternative02Address02 => ctrlAlternative02Address02;
        public TextBox ControlAlternative02Address03 => ctrlAlternative02Address03;
        public DropDownList ControlAlternative02Country => ctrlAlternative02Country;
        public DropDownList ControlAlternative02State => ctrlAlternative02State;
        public DropDownList ControlAlternative02City => ctrlAlternative02City;
        public TextBox ControlAlternative02Phone01 => ctrlAlternative02Phone01;
        public TextBox ControlAlternative02Remark => ctrlAlternative02Remark;

        #endregion
    }
}
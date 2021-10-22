using System;
using System.Windows.Controls;

namespace SysAdmin.Dialogs
{
    /// <summary>
    /// Interaction logic for AddDepartment
    /// </summary>
    public partial class AddDepartment : UserControl
    {
        public AddDepartment()
        {
            InitializeComponent();
        }

        private void treeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var a = e.NewValue as SysOrganizeTree;
            if (a?.id != (combobomtree.Items[0] as SysOrganizeTree)?.id)
            {
               combobomtree.Items[0] = a;
            }

        }

        private void combobomtree_DropDownClosed(object sender, EventArgs e)
        {         
            combobomtree.SelectedItem = combobomtree.Items[0];
        }
    }
}

﻿

#pragma checksum "C:\Users\thedirewolf7\Documents\Visual Studio 2013\Projects\AttendancePrototype1\AttendancePrototype1\AttendancePrototype1.Windows\Pages\Langing2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E8D271E273560C793048CAB46D1425AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AttendancePrototype1.Pages
{
    partial class Langing2 : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 11 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).SizeChanged += this.pageRoot_SizeChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 98 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.SubjectGridView_ItemClick;
                 #line default
                 #line hidden
                #line 98 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.SubjectGridView_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 68 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.UpdateAttendance_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 78 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.MakeAttendanceUpdate_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 152 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Refresh_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 153 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Help_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 154 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Reset_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 157 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AddSubject_Click;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 158 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.EditSubject_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 159 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.DeleteSubject_Click;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 166 "..\..\..\Pages\Langing2.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.donechangename_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}



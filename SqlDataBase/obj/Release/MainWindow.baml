   M S B A M L   `   `   `  ���� F  BSqlDataBase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SqlDataBaseSqlDataBase  H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls     SqlDataBase.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:SqlDataBase     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     $��497.669��$��1096.458��  � Title$  SqlDataBase��(  "MahApps.Metro.Controls.MetroWindow    IsMaxRestoreButtonEnabled .     ShowMaxRestoreButton .   � 
ResizeMode$ CanMinimize=� � WindowStartupLocation$ CenterScreen=�$
��1024��$	��450��.��� .+��� -   $��ButtonLogout��$��
Выход��$��Right=�$	��Top=�$��55��$��0,5,7,0q�$��27��|�.  �� -   $��TabControlMain��$��Stretch=�.@��� -   $��TabItemHome��$��Главная��.��� .+�� $��	0,0,637,0q�.+��� -   $��ListViewStudent��$��Stretch=�$
��Left=�$	��347��$��	100,0,0,0q��� -   $��ListViewGroup��$��Stretch=�$
��Left=�$	��100��� -   $��GridDefaultUser��$y�Visible=�$��	447,0,0,0q�.+��� -   $��ButtonProfileDelete��$��Удалить��$��Right=�$	��Top=�$��75��$��
0,-37,64,0q�|�.  �� -   $��ButtonProfileSave��$��Сохранить��$��Right=�$	��Top=�$��75��$��0,-37,144,0q�|�.  �� -	   $��ButtonProfileReport��$��
Отчет��$��Right=�$	��Top=�$��75��$��0,-37,224,0q�|�.  	� -
   $��RadioButtonStudent��$��Студент��$
��Left=�$	��Top=�$��
10,350,0,0q� d� 	IsChecked$
 TrueY�	� -   $��RadioButtonDad��$��Отец��$
��Left=�$	��Top=�$��
10,373,0,0q�	� -   $��RadioButtonMom��$��Мать��$
��Left=�$	��Top=�$��
10,396,0,0q�� -   $��	GridPhoto��.+��� -   $��LabelImageAvatar��$��Center=�$��Фотография��$��Center=�$	��Top=�$��
10,0,389,0q�$	��238���� -   $��ButtonImageBack��   TriangleButtonStyle#���   $��11,87,585,286q��� -   $��ButtonImageNext��#���   $��211,87,388,286q� J� RenderTransformOrigin$ 0.5,0.5&�z�V� .~���  �� ScaleX$ -1Q��� �� T� A� $	��150��$	��150��$��	57,39,0,0q�$
��Left=�$	��Top=����� -   ImageAvatar���� -   $��ButtonChangeAvatar��$��Изменить��$
��Left=�$	��Top=�$��75��$��176,194,0,0q�|�.  �� -   $��ButtonDefaultAvatar��$��Сбросить��$
��Left=�$	��Top=�$��75��$��
16,194,0,0q�|�.  �� -   $��ButtonOpenFullImage��$��Открыть��$
��Left=�$	��Top=�$��75��$��
96,194,0,0q�|�.  � -   $��ListBoxStudentInfoCreate��$��Stretch=�$��Stretch=�$��	248,0,0,0q�.+��� $��Имя:��$
��Left=�$	��Top=�$��	74,10,0,0q��� $��Фамилия:��$
��Left=�$	��Top=�$��	48,41,0,0q��� $��Отчество:��$
��Left=�$	��Top=�$��	47,72,0,0q��� $ ��Дата рождения:��$
��Left=�$	��Top=�$��
13,103,0,0q��� $��Город:��$
��Left=�$	��Top=�$��
64,134,0,0q��� -   $��LabelGroupAndPlacework��$��Группа:��$
��Left=�$	��Top=�$��
59,165,0,0q��� $!��Дополнительно:��$
��Left=�$	��Top=�$��
10,258,0,0q��� -   $��TextBoxName��$
��Left=�$��23�� �� TextWrapping$
 Wrap=�$	��Top=�$	��268��$��
115,10,0,0q��� -   $��TextBoxSubname��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��
115,41,0,0q��� -   $��TextBoxPatronymic��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��
115,72,0,0q��� -   $��TextBoxLocation��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��115,134,0,0q��� -   $!��TextBoxGroupNameOrPlaceWork��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��115,165,0,0q�(  "System.Windows.Controls.DatePicker  -   $��DatePickerDateBirth��$
��Left=�$	��Top=�$��115,103,0,0q�|�.  $	��268���� -   $��TextBoxAdditional��$
��Left=�$
 Wrap=�	 �� AcceptsReturn	 . "
 �� VerticalScrollBarVisibility$

 Auto=�$��115,258,0,0q�$	��268��	� -   $��RadioButtonGenderMan��$��Мужской��$
��Left=�$	��Top=�$��169,231,0,0q�	� -   $��RadioButtonGenderWoman��$��Женский��$
��Left=�$	��Top=�$��249,231,0,0q��� $��Пол:��$
��Left=�$	��Top=�$��
75,227,0,0q��� $��Телефон:��$
��Left=�$	��Top=�$��
50,196,0,0q��� -    $��TextBoxPhone��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��115,196,0,0q��� -!   $��TabItemCreateStudent��$!��Создать анкету��.��� -"   $��GridMain��$��0,0,0,0q�.+��� -#   $��LabelCreateStep��$��Студент��$
��Left=�$	��Top=�$��36(�$��
Myriad Pro)�$��	10,10,0,0q�� -$   $��GridCreateInfo��$��Stretch=�$��Stretch=�$��338,0,290,0q�.+��� $��Имя:��$
��Left=�$	��Top=�$��	74,10,0,0q��� $��Фамилия:��$
��Left=�$	��Top=�$��	48,41,0,0q��� $��Отчество:��$
��Left=�$	��Top=�$��	47,72,0,0q��� $ ��Дата рождения:��$
��Left=�$	��Top=�$��
13,103,0,0q��� $��Город:��$
��Left=�$	��Top=�$��
64,134,0,0q��� -%   $"��LabelCreateGroupAndPlacework��$��Группа:��$
��Left=�$	��Top=�$��
59,165,0,0q��� -&   $��LabelCreateAdditional��$!��Дополнительно:��$
��Left=�$	��Top=�$��
10,258,0,0q��� -'   $��TextBoxCreateName��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��
115,10,0,0q��� -(   $��TextBoxCreateSubname��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��
115,41,0,0q��� -)   $��TextBoxCreatePatronymic��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��
115,72,0,0q��� -*   $��TextBoxCreateLocation��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��115,134,0,0q��� -+   $'��!TextBoxCreateGroupNameOrPlaceWork��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��115,165,0,0q�  -,   $��DatePickerCreateDateBirth��$
��Left=�$	��Top=�$��115,103,0,0q�|�.  $	��268���� --   $��TextBoxCreateAdditional��$
��Left=�$
 Wrap=�	 . $

 Auto=�$��115,258,0,0q�$	��268��	� -.   $ ��RadioButtonCreateGenderMan��$��Мужской��$
��Left=�$	��Top=�$��169,231,0,0q�	� -/   $"��RadioButtonCreateGenderWoman��$��Женский��$
��Left=�$	��Top=�$��249,231,0,0q��� -0   $��LabelCreateGender��$��Пол:��$
��Left=�$	��Top=�$��
75,227,0,0q��� $��Телефон:��$
��Left=�$	��Top=�$��
50,196,0,0q��� -1   $��TextBoxCreatePhone��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��268��$��115,196,0,0q��� -2   $��ButtonCreateContinue��$��Продолжить��$
��Left=�$	��Top=�$��92��$��982,188,0,0q�|�.  �� -3    ��Name$ ButtonCreateBack��$��Вернуться��$
��Left=�$	��Top=�$��92��$��
10,188,0,0q�|�.  {�.  �� -4   $��TabItemSettings��$��Настройки��.��� .+��� $��Логин:��$
��Left=�$	��Top=�$��
388,22,0,0q��� $��Имя:��$
��Left=�$	��Top=�$��
398,53,0,0q��� $��Фамилия:��$
��Left=�$	��Top=�$��
372,84,0,0q��� $��Новый пароль:��$
��Left=�$	��Top=�$��341,115,0,0q��� $*��$Подтвердите пароль:��$
��Left=�$	��Top=�$��309,146,0,0q��� -5   $��TextBoxSetLogin��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��286��$��
439,22,0,0q��� -6   $��TextBoxSetName��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��286��$��
439,53,0,0q��� -7   $��TextBoxSetSubname��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��286��$��
439,84,0,0q�G� -8   $��PasswordBoxSetPassword��$
��Left=�$	��Top=�$	��286��$��439,115,0,0q�G� -9   $#��PasswordBoxSetConfirmPassword��$
��Left=�$	��Top=�$	��286��$��439,146,0,0q��� -:   $��ButtonSetSaveSettings��$��Сохранить��$��Right=�$	��Top=�$��75��$��
0,-37,64,0q� � 	Resources   Geometry  FocusVisual �� Property �� Value �� StrokeDashArray  Button.Static.Background �� Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border 	 Button.Disabled.Background 
 Button.Disabled.Border �� 
TargetType �� 
TargetType �� Triggers P� Property P� Value �� 
TargetName  	Rotate090 �� CenterX �� CenterY �� Angle  	Rotate180  	Rotate270��%"  &	       &	      7  &	 l     &	 �     &	 �     &	 �     &	 �     &	 �     &		 �     &	
 �     &	      7  7  7  7  7  7  7  7  7 	 7 
 &	     &	 (    &	 I    ��M12,24 L36,0 L36,48 Z��� .���  � �� �� .R�F� #��� ��$��1��$	 1 2R�8��  $��48���� $ 	#FFDDDDDD���� $ 	#FF707070���� $ 	#FFBEE6FD���� $ 	#FF3C7FB1���� $ 	#FFC4E5F6���� $ 	#FF2C628B���� $ 	#FFF4F4F4���� $ 	#FFADB2B5���� " ��.���  �@�FocusVisualStyle8   ��  � ��8  ��  � ��8  ��  � �� �� " ��.R�F� $��border��#��z��#��z��8�� $��48�� P�  �@��IsDefaulted . . ��� $ border��# � �� � ��P�  �@J�IsMouseOver . . ��� $ border��8   � ���� $ border��8   � ��P�  � �� . . ��� $ border��8   � ���� $ border��8   � ��P�  � {� .  . ��� $ border��8   � ���� $ border��8 	  � ���� $ 24Q�$ 24Q�$	 090Q��� $ 24Q�$ 24Q�$	 180Q��� $ 24Q�$ 24Q�$	 270Q�
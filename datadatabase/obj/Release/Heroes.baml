   M S B A M L   `   `   `  ���� G  Cdatadatabase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:datadatabasedatadatabase      datadatabase.Heroes   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:datadatabase       N� Title$  
MainWindow��$	��690��$	��910��.��� $��0,-21,3.6,-20q�.+��� -    ��Name$ SubmitButton��$��Submit��$
��Left=�$��
70,432,0,0q�$	��Top=�$��85�� J� RenderTransformOrigin$ 0.5,0.5&�$��26��$��Times New Roman)�$��18(�z�V� .~��� �� ��  �� Angle$ 0.795Q�T� &   System.Windows.Controls.DataGrid  -     Name$ DataGrid��$
��Left=�$	��658��$	��Top=�$	��566��$��
311,27,0,0q�   CanUserAddRows .     CanUserDeleteRows .     CanUserReorderColumns .     CanUserResizeColumns .  	   CanUserResizeRows	 .  
   CanUserSortColumns
 . �� -    ��Name$ OperationCombo��$
��Left=�$	��Top=�$��90��$��	10,63,0,0q�$��32��$��Times New Roman)�$��18(�.@��� $��Select���� $��Delete���� $��Update���� $��Insert���� $��Hero(es) by��$
��Left=�$	��Top=�$��
105,63,0,0q�$��32��$	��100��$��Times New Roman)�$��18(��� -   $ 	AttrCombo��$
��Left=�$	��Top=�$��92��$��
205,63,0,0q�$��32��$��Times New Roman)�$��18(�.@��� -    ��Name$ 	_combo_id��$��Id���� $
��Name���� -   $ _combo_attr��$��	Main Attr���� -   $ _combo_attack��$��Attack Type���� -    ��Name$ DependentLabel��$�� ��$
��Left=�$	��Top=�$��
10,122,0,0q�$��33��$��90��$��Times New Roman)�$��18(��� -	    ��Name$ 	TextQuery��$
��Left=�$��33�� �� TextWrapping$
 Wrap=�$	��Top=�$	��178��$��105,122,0,0q�$��Times New Roman)�$��18(��� -
   $ RollBack��$��RollBack��$
��Left=�$	��Top=�$��85��$��
70,401,0,0q�$��26��$��Times New Roman)�$��18(��� -   $ Commit��$��Commit��$
��Left=�$��
70,368,0,0q�$	��Top=�$��85��$ -0.117,0.76&�$��28��$��Times New Roman)�$��18(��� -   $��ToSpells��$
��Left=�$��
70,632,0,0q�$	��Top=�$��85��$��27��$��Times New Roman)�$��16(��� -    ��Name$ Level��$
��Left=�$��
90,188,0,0q�$	��Top=�$��34��$	��207��$��Times New Roman)�$��22(� � Maximum$ 30Q� �� IsSnapToTickEnabled .  �� TickPlacement$ BottomRight=� �� AutoToolTipPlacement$ BottomRight=� �� AutoToolTipPrecision$ 2���� $��Level��$
��Left=�$��
10,188,0,0q�$	��Top=�$��75��$��Times New Roman)�$��16(��� -   $��Relog��$
��Left=�$��208,632,0,0q�$	��Top=�$��75��$��28��$��Times New Roman)�$��16(�
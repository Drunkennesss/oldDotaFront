   M S B A M L   `   `   `  ����G  Cdatadatabase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:datadatabasedatadatabase  5   	       datadatabase.Heroes   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:datadatabase     5   	     N� Title$  
MainWindow��5   	   $	��690��6   $	��910��6)   .��5	      � $��0,-21,3.6,-20q�6   .+�5
   
   �� -   5         ��Name$ SubmitButton��5
      $��Submit��6'   $
��Left=�68   $��
70,432,0,0q�6S   $	��Top=�6g   $��85��6    J� RenderTransformOrigin$ 0.5,0.5&�6�   $��26��6�   $��Times New Roman)�6�   $��18(�6�   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ 0.795Q�6&   T� 5      5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ DataGrid��5      $
��Left=�6%   $	��658��6@   $	��Top=�6M   $	��566��6e   $��
311,27,0,0q�6q      CanUserAddRows .     CanUserDeleteRows .     CanUserReorderColumns .     CanUserResizeColumns .  	   CanUserResizeRows	 .  
   CanUserSortColumns
 . 6  �� 5   
   -   5         ��Name$ OperationCombo��5      $
��Left=�6+   $	��Top=�6F   $��90��6^   $��	10,63,0,0q�6i   $��32��6|   $��Times New Roman)�6�   $��18(�6�   .@�5      �� $��Select��6   �� 5      $��Delete��6   �� 5      $��Update��6   �� 5      $��Insert��6   5      �� 5   
   $��Hero(es) by��6   $
��Left=�6&   $	��Top=�6A   $��
105,63,0,0q�6Y   $��32��6m   $	��100��6y   $��Times New Roman)�6�   $��18(�6�   �� 5   
   -   5        $ 	AttrCombo��5      $
��Left=�6&   $	��Top=�6A   $��92��6Y   $��
205,63,0,0q�6d   $��32��6x   $��Times New Roman)�6�   $��18(�6�   .@�5      �� -   5         ��Name$ 	_combo_id��5      $��Id��6.   �� 5      $
��Name��6   �� 5      -   5        $ _combo_attr��5      $��	Main Attr��60   �� 5       -   5        $ _combo_attack��5       $��Attack Type��62   5!      �� 5"   
   -   5         ��Name$ DependentLabel��5"      $�� ��6(   $
��Left=�63   $	��Top=�6N   $��
10,122,0,0q�6f   $��33��6z   $��90��6�   $��Times New Roman)�6�   $��18(�6�   �� 5#   
   -	   5         ��Name$ 	TextQuery��5#      $
��Left=�6%   $��33��6@    �� TextWrapping$
 Wrap=�6L   $	��Top=�6`   $	��178��6x   $��105,122,0,0q�6�   $��Times New Roman)�6�   $��18(�6�   �� 5$   
   -
   5        $ RollBack��5$      $��RollBack��6#   $
��Left=�66   $	��Top=�6Q   $��85��6i   $��
70,401,0,0q�6t   $��26��6�   $��Times New Roman)�6�   $��18(�6�   �� 5%   
   -   5        $ Commit��5%      $��Commit��6!   $
��Left=�62   $��
70,368,0,0q�6M   $	��Top=�6a   $��85��6y   $ -0.117,0.76&�6�   $��28��6�   $��Times New Roman)�6�   $��18(�6�   6�   �� 5&   
   -   5        $��ToSpells��5&      $
��Left=�6$   $��
70,632,0,0q�6?   $	��Top=�6S   $��85��6k   $��27��6v   $��Times New Roman)�6�   $��16(�6�   6�   �� 5'   
   -   5         ��Name$ Level��5'      $
��Left=�6    $��
90,188,0,0q�6;   $	��Top=�6O   $��34��6g   $	��207��6s   $��Times New Roman)�6   $��22(�6�    � Maximum$ 30Q�6�    �� IsSnapToTickEnabled .  �� TickPlacement$ BottomRight=�6�    �� AutoToolTipPlacement$ BottomRight=�6�    �� AutoToolTipPrecision$ 2��6  �� 5(   
   $��Level��6   $
��Left=�6    $��
10,188,0,0q�6;   $	��Top=�6O   $��75��6g   $��Times New Roman)�6r   $��16(�6�   �� 5)   
   -   5        $��Relog��5)      $
��Left=�6!   $��208,632,0,0q�6<   $	��Top=�6Q   $��75��6i   $��28��6t   $��Times New Roman)�6�   $��16(�6�   6�   5*      5+      
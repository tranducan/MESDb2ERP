   M S B A M L   `   `   `  ����F  BMesDb2ERPDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MesDb2ERPDb.ViewMesDb2ERPDb.View  5   	   #    MesDb2ERPDb.View.WindowConfig   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:MesDb2ERPDb.View     5   	     � Title$  WindowConfig��5   	   $��326.4��6   $	��713��6-   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��160* �6   �� 5      $
��257* �6   .+�5   
   �� -   5         ��Name$ txt_serverOffline��5      $��33��6-   $��260,20,73.6,0q�69    �� TextWrapping$
 Wrap=�6P   $	��Top=�6d   �� 5   
   -   5         ��Name$ cb_serveroffline��5      $��using server offline��6-   $
��Left=�6L   $��	70,44,0,0q�6g   $	��Top=�6z   � 5   
   $��0,0,-0.4,-0.2q�6   $��2��6&    � ColumnDefinitions 5      �� 5      $
��120* �6#   �� 5      $
��105* �6#   �� 5      $
��172* �6#    5      �� 5      $	��70* �6    �� 5      $	��45* �6    �� 5      $	��65* �6    �� 5      $	��47* �6    �� 5      $	��70* �6    .+�5      �� -   5        $ txt_userOffline��5      $��34��6/   $��31.2,5.6,73.4,0q�6;   $
 Wrap=�6T   $	��Top=�6h   $��2��6�   $��1��6�   �� 5      $��Server Offline��6   $��
60,13,70,0q�6-   $	��Top=�6A   $��26��6Y   �� 5!      $
��User��6   $��1��6#   $��105.4,11.6,12.8,0q�63   $��1��6N   $	��Top=�6[   $��26��6s   �� 5#      $��Password��6   $��1��6'   $��105.4,14.8,16.2,0q�67   $��2��6R   $	��Top=�6_   $��26��6w   �� 5&      -   5         ��Name$ 	btn_Save1��5&      $
��Save��6(   $��2��67   $��205.4,0,43.6,9.8q�6G   $��4��6a   $��37��6�   $��Bottom=�6�    J� RenderTransformOrigin$ 	0.5,0.505&�6�   ��5'      �� 5(       �� EndPoint$ 0.5,1&�6*   	 �� 
StartPoint$	 0.5,0&�6;   .��5)      � 
 � Color$
 	#FFE3F04E��6'    � Offset$ 0Q�69   � 5*      $
 	#FF34BF8F��6'   $ 1Q�69   5+      5-      G� 5.      -   5         G�Name$ passwordBox��5.      $��2��6/   $��30.8,0,73.2,22q�6?   $��2��6W   $��Bottom=�6d   $��33��6   �� 5/      $��Timer��6   $��1��6$   $��105.4,8,3,4.2q�64   $��3��6K   �� 50      -   5        $ 
txt_timmer��50      $��34��6*   $��30.8,7,179.2,0q�66   $
 Wrap=�6N   $	��Top=�6b   $��3��6z   $��2��6�   51      54      55      
   M S B A M L   `   `   `  ����A  =Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ServerServer  5   	       Server.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Server     5   	   V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  5   	      MaterialDesignBody#���   5	   	     MaterialDesignPaper#���  5
   	   $��Medium"�5   	   $��20(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	    �� BitmapScalingMode$ HighQuality=�5   	   $��/Font/#r0c0i Linotte)�5   	    � Title$ 
MainWindow��5   	   $	��690��6   $	��989��6)    � 
ResizeMode$ NoResize=�65    � WindowStyle$
 None=�6K    � WindowStartupLocation$ CenterScreen=�6^   .��5      � ���	#FF2F3242.+�5   
   )  #MaterialDesignThemes.Wpf.DialogHost  -   5        $
��bug1��5         IsOpen .     DialogContent 5      �� 5      $��30q�6   .+�5      �� $H��BLỗi: Không thể đóng Server trước khi các Client thoát!��6    $
��Wrap=�6^   $0��*/Client;component/Font/r0c0i Linotte Light)�6r   $��20(�6�   $
��Auto��6�    �� 
LineHeight$	 1.8��6�   6�   �� 5      -   5        $��close_newreg_popup_1��5      $��Đóng��69     MaterialDesignFlatButton#���  6H   $��Center=�6{   $��r0c0i Linotte Light)�6�   $��20(�6�   $��0,15,0,0q�6�   6�   5      5      �� 5   
   $
��Left=�6   $��60��6+   $��	32,12,0,0q�67   $	��Top=�6J   $	��168��6b   $��/Image/Logo_Server.png��6n   $
��None=�6�   �� 5   
   $��Visible=�6   $��32,83,382,0q�5      .��5      �� -   5        	 ��Name$	 WPFLog��5      $
��Left=�5       $	��592��5!      $	��Top=�5"      $	��575��5#      ���	#FF2B2D3C
 �� Orientation$
 Vertical=�5%      5&      5'      �� 5(   
   $
��Left=�6   $��
629,49,0,0q�6+   $��Online��6?   $	��Top=�6P   
�������$��20(�6{   $��r0c0i Linotte)�6�   �� 5*   
   $��Visible=�6   $��629,85,28,75q�5+      .��5,      �� -   5        $	 	WPFOnList��5,      ���	#FF2B2D3C$
��Left=�5.      $	��530��5/      $	��Top=�50      $	��332��51      $
 Vertical=�52      53      54      �� 56   
   -   5         ��Name$ WPFStart��56       " MaterialDesignRaisedDarkButton#���  6#   $��Start Server��6\   $
��Left=�6s   $��54��6�   $��629,620,0,0q�6�   $	��Top=�6�   $	��332��6�   $��20(�6�   $+��%/Server;component/Font/#r0c0i Linotte)�6�   $��0q�6+  �� 57   
   $
��Left=�6   $��
565,49,0,0q�6+   $	��Log��6?   $	��Top=�6M   
�������$��20(�6x   $��r0c0i Linotte)�6�   �� 58   
   -   5        $ Close��58      $
��Left=�6    $��41��6;   $��
931,10,0,0q�6G   $	��Top=�6[   $��48��6s   #���  6~   .��59      �� $��/Image/close_icon.png��6   5:      �� 5;   
   -   5        $ Minimize��5;      $
��Left=�6#   $��41��6>   $��
883,10,0,0q�6J   $	��Top=�6^   $��48��6v   #���  6�   .��5<      �� $��/Image/minimize_icon.png��6   5=      �� 5>   
   -   5        $
 Drag��5>      $
��Left=�6   $��41��6:   $��
835,10,0,0q�6F   $	��Top=�5?      $��48��6%   #���  60   $��9q�5@      .��5C      �� $��/Image/DragMove.png��6   63   5D      5E      5F      
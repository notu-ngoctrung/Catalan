   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	       Client.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	   V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  5   	      MaterialDesignBody#���   5   	     MaterialDesignPaper#���  5	   	   $��Medium"�5
   	   $��20(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	    �� BitmapScalingMode$ HighQuality=�5   	   $��/Font/#r0c0i Linotte)�5   	    � Title$ 
MainWindow��5   	   $	��690��6   $	��989��6)    � WindowStartupLocation$ CenterScreen=�65    � 
ResizeMode$ NoResize=�6Z    � WindowStyle$
 None=�6p   .��5      � ��5   
   �� 5       �� ImageSource$ /Image/Login.png��6   $ HighQuality=�68   .+�5   
   )  #MaterialDesignThemes.Wpf.DialogHost  -   5        $��Newreg_Popup_1��5         DialogContent 5      �� 5      $��30q�6   .+�5      �� $����Lỗi: Để đăng ký tài khoản mới, bạn cần nhập tên người dùng mong muốn và điền vào khung mật khẩu bên dưới.��6    $
��Wrap=�6�   $0��*/Client;component/Font/r0c0i Linotte Light)�6�   $��20(�6�   $	��600��6�    �� 
LineHeight$	 1.8��6�   6  �� 5      -   5        $��close_newreg_popup_1��5      $��Đóng��69     MaterialDesignFlatButton#���  6H   $��Center=�6{   $��r0c0i Linotte Light)�6�   $��20(�6�   $��0,15,0,0q�6�   6�   5      5        5    
   -   5        $��Newreg_Popup_2��5        5!      �� 5"      $��30q�6   .+�5#      �� $V��PLỗi: Bạn đã có đăng ký tài khoản trong hệ thống trước đây.��6    $
��Wrap=�6b   $0��*/Client;component/Font/r0c0i Linotte Light)�6v   $��20(�6�   $	��500��6�   $	 1.8��6�   6�   �� 5$      -   5        $��close_newreg_popup_2��5$      $��Đóng��69   #���  6H   $��Center=�6{   $��r0c0i Linotte Light)�6�   $��20(�6�   $��0,15,0,0q�6�   6�   5%      5'        5)   
   -   5        $��Newreg_Popup_3��5)       5*      �� 5+      $��30q�6   .+�5,      �� $l��fĐăng ký tài khoản mới thành công! Xin đóng popup này và đăng nhập vào hệ thống!��6    $
��Wrap=�6w   $0��*/Client;component/Font/r0c0i Linotte Light)�6�   $��20(�6�   $	��500��6�   $	 1.8��6�   6�   �� 5-      -   5        $��close_newreg_popup_3��5-      $��Đóng��69   #���  6H   $��Center=�6{   $��r0c0i Linotte Light)�6�   $��20(�6�   $��0,15,0,0q�6�   6�   5.      50        52   
   -   5        $��Signin_Popup_1��52       53      �� 54      $��30q�6   .+�55      �� $Y��SLỗi: Bạn chưa đăng ký tài khoản hoặc nhập username và password sai!��6    $
��Wrap=�6k   $0��*/Client;component/Font/r0c0i Linotte Light)�6   $��20(�6�   $	��500��6�   $	 1.8��6�   6�   �� 56      -   5        $��close_signin_popup_1��56      $��Đóng��69   #���  6H   $��Center=�6{   $��r0c0i Linotte Light)�6�   $��20(�6�   $��0,15,0,0q�6�   6�   57      59        5;   
   -	   5        $��Signin_Popup_2��5;       5<      �� 5=      $��30q�6   .+�5>      �� $!��Đăng nhập thành công!��6    $
��Wrap=�6=   $0��*/Client;component/Font/r0c0i Linotte Light)�6Q   $��20(�6�   $	 1.8��6�   6�   �� 5?      -
   5        $��close_signin_popup_2��5?      $��Đóng��69   #���  6H   $��Center=�6{   $��r0c0i Linotte Light)�6�   $��20(�6�   $��0,15,0,0q�6�   6�   5@      5B        5C   
   -   5        $��Signin_Popup_3��5C       5D      �� 5E      $��30q�6   .+�5F      �� $G��AServer hiện chưa hữu dụng! Vui lòng kết nối lại sau��6    $
��Wrap=�6[   $0��*/Client;component/Font/r0c0i Linotte Light)�6o   $��20(�6�   $	 1.8��6�   6�   �� 5G      -   5        $��close_signin_popup_3��5G      $��Đóng��69   #���  6H   $��Center=�6{   $��r0c0i Linotte Light)�6�   $��20(�6�   $��0,15,0,0q�6�   6�   5H      5J      �� 5M   
   -   5        	 ��Name$	 HostIP��5M      $��Center=�6"   $��55��6?   $��326,359,327,0q�6K   
 �� TextWrapping$

 Wrap=�6b   $)��#Nhập địa chỉ IP của Server��6v   $	��Top=�6�   $	��336��6�   $��r0c0i Linotte Light)�6�   �� 5N   
   -   5        $	 Username��5N      $��Center=�6$   $��55��6A   $��326,427,327,0q�6M   $

 Wrap=�6d   $ ��Nhập tên người dùng��6x   $	��Top=�6�   $	��336��6�   $��r0c0i Linotte Light)�6�   6�   G� 5O   
   -   5         G�Name$ Password��5O      $
��Left=�6(   $��55��6C   $��326,495,0,0q�6O   $	��Top=�6d   $	��336��6|   6�   �� 5P   
   -   5         ��Name$ Signin��5P      $��Đăng nhập��6!    " MaterialDesignRaisedDarkButton#���  65   $��Center=�6n   $��50��6�   $��310,600,537,0q�6�   $	��Top=�6�   $	��142��6�   $��20(�6�   $��r0c0i Linotte Light)�6�   6  �� 5Q   
   -   5        $ Newreg��5Q      $��Đăng ký mới��6!   #���  67   $��Center=�6p   $��50��6�   $��474,600,314,0q�6�   $	��Top=�6�   $	��201��6�   $��20(�6�   $��r0c0i Linotte Light)�6�   6  �� 5R   
   -   5        $ Close��5R      $
��Left=�6    $��41��6;   $��
931,10,0,0q�6G   $	��Top=�6[   $��48��6s   #���  6~   .��5S      �� $��/Image/close_icon.png��6   65   5T      �� 5U   
   -   5        $ Minimize��5U      $
��Left=�6#   $��41��6>   $��
883,10,0,0q�6J   $	��Top=�6^   $��48��6v   #���  6�   .��5V      �� $��/Image/minimize_icon.png��6   68   5W      �� 5X   
   -   5        $
 Drag��5X      $
��Left=�6   $��41��6:   $��
835,10,0,0q�6F   $	��Top=�5Y      $��48��6)   #���  64   $��9q�5Z      .��5]      �� $��/Image/DragMove.png��6   63   5^      5`      5a      
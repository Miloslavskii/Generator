   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.5.0, Culture=neutral, PublicKeyToken=3b585c2a5f1a92c1G4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	    Generator_1.5.2"   Generator_1._5._2.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   T PMahApps.Metro, Version=1.4.3.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;controls,http://metro.mahapps.com/winfx/xaml/controls  5   	   >wpf4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf  5   	   -   5        $��MetroWnd��5        � Title$  	Generator��6	   $	��500��6+   $	��700��68   $	��250��6D   $	��400��6T      AccentColorBrush(  "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �   5      $��1q�5       � 
ResizeMode$ CanResizeWithGrip=�5	         WindowTransitionsEnabled .   � WindowStartupLocation$ CenterScreen=�5       � Icon$ Resources/G2 64.ico��5       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Resources/Icons.xaml?�6%   %    5      	   RightWindowCommands	 5      +  %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5        $��ComandSettingBtn��5      {�.  .��5      �� 
 �� Orientation$
 
Horizontal=�6   .+�5      �� $��20��6    $��20��6+   ��5      ��  �� RelativeSource ��  �� AncestorType$ ButtonL� �� Path$ 
Foreground� J� OpacityMask 5      !� 5       j� Stretch$
 Fill=�6*     
appbar_cog !� Visual# [ 69   5      �� 5       $��4 0 0 0q�6    $��Center=�5!      $��Settings��5"      5#      5$      �� 5%       � ToolTip$ Generator_1.5.2��6   .��5&      �� $
 
Horizontal=�6   .+�5'      �� $��20��6    $��20��6+   ��5(      ��  �� $ ButtonL�$ 
Foreground� 5)      !� 5*      $
 Fill=�6*     appbar_lightbulb_coil# [ 69   5,      �� 5-      $��4 0 0 0q�6    $��Center=�5.      5/      50      51      .��54      �  � RowDefinitions 55      �� 56      $��15 �6   �� 57      $��70 �6   �� 58      $��* �6    � ColumnDefinitions 5;   
   �� 5<      $��70 �6   �� 5=      $��70 �6   �� 5>      $��* �6   �� 5?      $��10 �6   .+�5B   
   #   LiveCharts.Wpf.CartesianChart  -   5        $��Chart��5B      $��0��6N   $��4��6^   $��2��6r   &    LiveCharts.Wpf.Charts.Base.Chart   Series 6*   �� *SeriesCollection+6r   �� 5D   
   -   5        $��LoadBtn��5D      $��0��6    $��1��60   $ 
Load Ticks��6S     MetroCircleButtonStyle#���  6i   .��5E      �� $
 
Horizontal=�6   .+�5F      �� $#��Resources/appbar.inbox.in.png��6   5G      5H      �� 5I   
   -   5        $��LearnBtn��5I      $��1��6!   $��1��61   $ Process Signals��6U   #���  6o   .��5J      �� $
 
Horizontal=�6   .+�5K      �� $(��"Resources/appbar.graph.line.up.png��6   5L      5M      -  'MahApps.Metro.Controls.MetroProgressBar  5O   
   -   5        $��ProgressBar��5O   %   $��2��68   $��1��6H   
����·�$��35��6j   $��Center=�6v    � IsIndeterminate .  
��� E��5P   %   )  #MahApps.Metro.Controls.ProgressRing  5Q   
   -   5        $��ProgressRing��5Q       $��0��64   $��3��6A   $��3��6U   $��0��6g      IsActive .  $��Center=�6�   $��Center=�6�   
��� E��5R       5S      5T      
   M S B A M L   `   `   `  ����M  IApplicationExample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ApplicationExampleApplicationExample  5   	   %    ApplicationExample.ResizeCanvas   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ApplicationExample     5   	   $	��300��5   	   $	��300��6     � Title$  ResizeCanvas��6"   .��5	      � $��20q�6    � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��80 �6   �� 5      $	��180 �6   .+�5   
   �� $��Resize your Canvas:��6   $��2��6.   $��Center=�6B   $��Center=�6f   �� 5   
   $��Width:��6   $��1��6!   $��Center=�6.   �� 5   
   -   5         ��Name$ textBoxCanvasWidth��5      $��1��6.   $��1��6;   $��10q�6K   $��Center=�6W   $��Center=�6{   �� 5   
   $��Height:��6   $��2��6"   $��Center=�6/   �� 5   
   -   5        $ textBoxCanvasHeight��5      $��2��6/   $��1��6<   $��10q�6L   $��Center=�6X   $��Center=�6|   �� 5   
   -   5         ��Name$ buttonUpdate��5      $��Resize��6'   $��3��68   $��10q�6E   $��60��6Q   6\   �� 5   
   -   5        $ buttonCancel��5      $��Cancel��6'   $��3��68   $��1��6E   $��10q�6U   $��60��6a   $��Center=�6l   $
��Left=�6�   5      5      
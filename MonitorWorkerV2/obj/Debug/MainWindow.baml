   M S B A M L   `   `   `  ����J  FMonitorWorkerV2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MonitorWorkerV2MonitorWorkerV2  5   	        MonitorWorkerV2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:MonitorWorkerV2     5   	     � Title$  
MainWindow��5   	   $	��682��6   $
��1024��6)    � 	Resources 5	       �� 
TargetType �� Property �� Value�� 5
   
   %�   '��       '�� H     �� 5      " ��6   .�5      ��  � ��$ 10q�6+   5      �� 5      " ��6   .�5      ��  � ��
 ���� 6/   �� 5       � ��
 ���� 60   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��2.5* �6   �� 5      $��* �6   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $��3* �6!   �� 5      $��1* �6!   .+�5      �� $��/Resources/Logo.jpg��6   $��1��61   $	��100��6>   $	��350��6K   $
��Left=�6W   $��30q�6r   6�   �� 5       -   5        $��winCatch��5    &   $��0��6   68   5!      � 5"   
   $��1��6    5#      �� 5$      $��* �6    �� 5%      $��* �6    �� 5&      $��* �6    �� 5'      $��* �6    .+�5*      �� $��
20,10,20,0q�6   $��0��6.   $��Center=�6;   .+�5,      �� ��6   ��  �� Path$ UserText�6<   �� 5-      ��6   �� $ WawiUserName�6?   �� 5.      ��6   �� $ PassText�6<   �� 5/      ��6   �� $ WawiPw�6:   50      �� 52      $��	10,0,20,0q�6    �� Orientation$ Vertical=�6,   $��1��6C   $��Center=�6P   .+�53      �� $$��Keep credentials after restart��6S   	 d� 	IsChecked	 6   �� $ KeepCredentials�
 �� Mode$
 TwoWay=�6~   �� 54      $.��(Restart JTL-Worker on hang and every 24h��6R   	 6   �� $ Restart24Hours�$
 TwoWay=�6�   �� 55      $#��Notify me per Email on Errors��6U   	 6   �� $ NotifyEmailErrors�$
 TwoWay=�6   �� 56      $��	10,10,0,0q�6B   ��6   �� $ Email�$
 TwoWay=�6W   57      �� 59      $��2��6   $	��100��6!   $	��100��6.   $��Stretch=�6:   ��6X   �� $ ProcessRunIco�6�   �� 5;      $��	0,0,30,40q�6   $��3��6,   $��Bottom=�69   .+�5=      �� $ 
Horizontal=�6   $��Right=�67   .+�5>      �� $��Right=�6   $��50��69   $��50��6D   .��5?      �� ��6    �� $ ShutdownIco�5@      �� 5A      $��Right=�6   $��50��69   $��50��6D   .��5B      �� ��6    �� $ DisconnectIco�5C      5D      5E      5G      5H      5I      
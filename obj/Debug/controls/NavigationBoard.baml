   M S B A M L   `   `   `  ����M  IFlightSimulatorWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:FlightSimulatorWpf.controlsFlightSimulatorWpf.controls  5      1    +FlightSimulatorWpf.controls.NavigationBoard   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:FlightSimulatorWpf.controls     5      .��5	      �� $��	0,0,443,0q�6   .+�5
   
   ]� $	��Top=�6   $	��339��6,     �� Left$  85��68   .+�5      �� $��Aileron��6   $��18(�6&   $
��Bold"�64   $��Center=�6G   $	��Top=�6k   $��
0,0,50.4,0q�6�   �� 5      -   5         ��Name$ aileronSlider_Value��5      $��Right=�63    �� TextWrapping$
 Wrap=�6J   $
��Left=�6^   $��50��5      $��38��6   ��6)   ��  �� ElementName$ aileron_slider�� �� Path$ Value��� 5      -   5         ��Name$ aileron_slider��5      
������� � Minimum$ -1Q�6>    � Maximum$ 1Q�6K    �� TickPlacement$ BottomRight=�6W   	 �� TickFrequency$		 0.1Q�6s   $��38��5      $	��280��6!   $
��Left=�6-   $
��Left=�6H   
 � Value
 6^   �� *
VM_Aileron+5      ]� 5   
   
 �� Top$ 32��6   $	��339��6$   .+�5      �� -   5        $ throttleSlider_Value��5      $
 Wrap=�6m   $��Bottom=�6�   $��50��5      $��38��6   $��Right=�6)   $��Center=�6F    �� TextAlignment$
 Left=�6a    �� IsReadOnlyCaretVisible .  J� RenderTransformOrigin$ 0.5,0.5&�5      $��0 0 -7 7q�66   ��5   4   �� $ throttle_slider��$ Value�.��5   H   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$	 -90Q�6*   T� 5      5      .��5   +   5      �� 5       -   5        $ throttle_slider��5       
�������$ 0Q�6H   $ 1Q�6T   $ BottomRight=�6`   $		 0.1Q�6|    �� Orientation$ Vertical=�6�   $	��280��5!      $��38��6"   $	��top=�6.   $��Right=�6F   $��0 0 0 0q�6]   
 6n   �� *VM_Throttle+�� 5"      $��Throttle��6   $
��Left=�6'   $��18(�6=   $
��Bold"�6K   $��85��6]   $��0,175,-50,51.8q�6h   $��35��6�   z�5#      V� 5$      .~�5%      �� �� 5&      �� 5'      $	 -90Q�6*   T� 5(      5)      5+      5,      *   $FlightSimulatorWpf.controls.Joystick  5.   
   -   5          Name$ MyJoys��5.      $ 85��6)   $	  110��69      Rudder 5/      �� *	VM_Rudder+ �� Mode$ OneWayToSource=�   Elevator 50      �� *VM_Elevator+$ OneWayToSource=�z�51      V� 52      .~�53      ��  �� ScaleX$
 0.65Q�6%    �� ScaleY$
 0.65Q�63   �� 54      �� 55      T� 56      57      59      �� 5;   
   $��18(�61   $ 0.5,0.5&�6?   $	  266��6_   $	 195��6q   $	��178��6�    �� TextAlignment$ Center=�6�   ��6   �� *VM_Elevator+z�5<      V� 5=      .~�5>      �� �� 5?      �� 5@      $ 90Q�6&   T� 5A      5B      5D      �� 5F   
   $��18(�6/   $	  137��6=   $	 314��6O   $	��178��6`   $ Center=�6l   ��6   �� *	VM_Rudder+6l   �� 5H   
   $��
<-Rudder->��6   $��18(�6+   $
��Bold"�69   $��Center=�6K   $	 338��6o   $	  165��6�   �� 5I   
   $��<-Elevator->��6   $��18(�6-   $
��Bold"�6;   $��Center=�6M   $��Center=�6q   $ 0.5,0.5&�5J      $	 192��60   $	  339��6A   z�5K      V� 5L      .~�5M      �� $
 1.15Q�6%   �� 5N      �� 5O      $ 90Q�6&   T� 5P      5Q      5S      5V      5X      
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace Carbine.Input
{
    public class InputManager
    {
        public static InputManager Instance
        {
            get
            {
                if (InputManager.instance == null)
                {
                    InputManager.instance = new InputManager();
                }
                return InputManager.instance;
            }
        }

        public event InputManager.ButtonPressedHandler ButtonPressed;

        public event InputManager.ButtonReleasedHandler ButtonReleased;

        public event InputManager.AxisPressedHandler AxisPressed;

        public event InputManager.AxisReleasedHandler AxisReleased;

        public Dictionary<Button, bool> State
        {
            get
            {
                return this.currentState;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }

        public Vector2f Axis
        {
            get
            {
                float x = Math.Max(-1f, Math.Min(1f, this.xAxis + this.xKeyAxis));
                float y = Math.Max(-1f, Math.Min(1f, this.yAxis + this.yKeyAxis));
                return new Vector2f(x, y);
            }
        }

        private InputManager()
        {
            this.currentState = new Dictionary<Button, bool>();
            foreach (object obj in Enum.GetValues(typeof(Button)))
            {
                Button key = (Button)obj;
                this.currentState.Add(key, false);
            }
            this.enabled = true;
        }

        public void AttachToWindow(Window window)
        {
            window.SetKeyRepeatEnabled(false);
            window.JoystickButtonPressed += this.JoystickButtonPressed;
            window.JoystickButtonReleased += this.JoystickButtonReleased;
            window.JoystickMoved += this.JoystickMoved;
            window.JoystickConnected += this.JoystickConnected;
            window.JoystickDisconnected += this.JoystickDisconnected;
            window.KeyPressed += this.KeyPressed;
            window.KeyReleased += this.KeyReleased;
        }

        public void DetachFromWindow(Window window)
        {
            window.JoystickButtonPressed -= this.JoystickButtonPressed;
            window.JoystickButtonReleased -= this.JoystickButtonReleased;
            window.JoystickMoved -= this.JoystickMoved;
            window.JoystickConnected -= this.JoystickConnected;
            window.JoystickDisconnected -= this.JoystickDisconnected;
            window.KeyPressed -= this.KeyPressed;
            window.KeyReleased -= this.KeyReleased;
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (this.keyMap.ContainsKey(e.Code))
            {
                Button button = this.keyMap[e.Code];
                if (this.enabled && !this.currentState[button] && this.ButtonPressed != null)
                {
                    this.ButtonPressed(this, button);
                }
                this.currentState[button] = true;
                return;
            }
            bool flag = false;
            switch (e.Code)
            {
                case Keyboard.Key.Left:
                    this.leftPress = true;
                    flag = true;
                    break;
                case Keyboard.Key.Right:
                    this.rightPress = true;
                    flag = true;
                    break;
                case Keyboard.Key.Up:
                    this.upPress = true;
                    flag = true;
                    break;
                case Keyboard.Key.Down:
                    this.downPress = true;
                    flag = true;
                    break;
            }
            this.xKeyAxis = (this.leftPress ? -1f : 0f) + (this.rightPress ? 1f : 0f);
            this.yKeyAxis = (this.upPress ? -1f : 0f) + (this.downPress ? 1f : 0f);
            if (this.enabled && flag && this.AxisPressed != null)
            {
                this.AxisPressed(this, this.Axis);
            }
        }

        private void KeyReleased(object sender, KeyEventArgs e)
        {
            if (this.keyMap.ContainsKey(e.Code))
            {
                Button button = this.keyMap[e.Code];
                if (this.enabled && this.ButtonReleased != null)
                {
                    this.ButtonReleased(this, button);
                }
                this.currentState[button] = false;
                return;
            }
            bool flag = false;
            switch (e.Code)
            {
                case Keyboard.Key.Left:
                    this.leftPress = false;
                    flag = true;
                    break;
                case Keyboard.Key.Right:
                    this.rightPress = false;
                    flag = true;
                    break;
                case Keyboard.Key.Up:
                    this.upPress = false;
                    flag = true;
                    break;
                case Keyboard.Key.Down:
                    this.downPress = false;
                    flag = true;
                    break;
            }
            this.xKeyAxis = (this.leftPress ? -1f : 0f) + (this.rightPress ? 1f : 0f);
            this.yKeyAxis = (this.upPress ? -1f : 0f) + (this.downPress ? 1f : 0f);
            if (this.enabled && flag && this.AxisReleased != null)
            {
                this.AxisReleased(this, this.Axis);
            }
        }

        private void JoystickMoved(object sender, JoystickMoveEventArgs e)
        {
            Joystick.Axis axis = e.Axis;
            switch (axis)
            {
                case Joystick.Axis.X:
                    this.xAxis = Math.Max(-1f, Math.Min(1f, e.Position / 70f));
                    if (this.xAxis > 0f && this.xAxis < 0.5f)
                    {
                        this.xAxis = 0f;
                    }
                    if (this.xAxis < 0f && this.xAxis > -0.5f)
                    {
                        this.xAxis = 0f;
                    }
                    break;
                case Joystick.Axis.Y:
                    this.yAxis = Math.Max(-1f, Math.Min(1f, e.Position / 70f));
                    if (this.yAxis > 0f && this.yAxis < 0.5f)
                    {
                        this.yAxis = 0f;
                    }
                    if (this.yAxis < 0f && this.yAxis > -0.5f)
                    {
                        this.yAxis = 0f;
                    }
                    break;
                default:
                    switch (axis)
                    {
                        case Joystick.Axis.PovX:
                            this.xAxis = Math.Max(-1f, Math.Min(1f, e.Position));
                            break;
                        case Joystick.Axis.PovY:
                            this.yAxis = Math.Max(-1f, Math.Min(1f, -e.Position));
                            break;
                    }
                    break;
            }
            this.axisZeroLast = this.axisZero;
            this.axisZero = (this.xAxis == 0f && this.yAxis == 0f);
            bool flag = this.axisZeroLast && !this.axisZero;
            if (this.enabled && flag && this.AxisPressed != null)
            {
                this.AxisPressed(this, this.Axis);
                return;
            }
            bool flag2 = !this.axisZeroLast && this.axisZero;
            if (this.enabled && flag2 && this.AxisReleased != null)
            {
                this.AxisReleased(this, this.Axis);
            }
        }

        private void JoystickConnected(object sender, JoystickConnectEventArgs e)
        {
            Joystick.Update();
            Joystick.Identification identification = Joystick.GetIdentification(e.JoystickId);
            Console.WriteLine("Gamepad {0} connected: {1} ({2}, {3})", new object[]
            {
                e.JoystickId,
                identification.Name,
                identification.VendorId,
                identification.ProductId
            });
        }

        private void JoystickDisconnected(object sender, JoystickConnectEventArgs e)
        {
            Console.WriteLine("Gamepad {0} disconnected", e.JoystickId);
        }

        private void JoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            if (!this.joyMap.ContainsKey(e.Button))
            {
                return;
            }
            Button button = this.joyMap[e.Button];
            this.currentState[button] = true;
            if (this.enabled && this.ButtonPressed != null)
            {
                this.ButtonPressed(this, button);
            }
        }

        private void JoystickButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            if (!this.joyMap.ContainsKey(e.Button))
            {
                return;
            }
            Button button = this.joyMap[e.Button];
            this.currentState[button] = false;
            if (this.enabled && this.ButtonReleased != null)
            {
                this.ButtonReleased(this, button);
            }
        }

        private const float DEAD_ZONE = 0.5f;

        private static InputManager instance;

        private Dictionary<Keyboard.Key, Button> keyMap = new Dictionary<Keyboard.Key, Button>
        {
            {
                Keyboard.Key.Z,
                Button.A
            },
            {
                Keyboard.Key.X,
                Button.B
            },
            {
                Keyboard.Key.S,
                Button.X
            },
            {
                Keyboard.Key.D,
                Button.Y
            },
            {
                Keyboard.Key.A,
                Button.L
            },
            {
                Keyboard.Key.F,
                Button.R
            },
            {
                Keyboard.Key.Return,
                Button.Start
            },
            {
                Keyboard.Key.BackSpace,
                Button.Select
            },
            {
                Keyboard.Key.Escape,
                Button.Escape
            },
            {
                Keyboard.Key.Tilde,
                Button.Tilde
            },
            {
                Keyboard.Key.F1,
                Button.F1
            },
            {
                Keyboard.Key.F2,
                Button.F2
            },
            {
                Keyboard.Key.F3,
                Button.F3
            },
            {
                Keyboard.Key.F4,
                Button.F4
            },
            {
                Keyboard.Key.F5,
                Button.F5
            },
            {
                Keyboard.Key.F6,
                Button.F6
            },
            {
                Keyboard.Key.F7,
                Button.F7
            },
            {
                Keyboard.Key.F8,
                Button.F8
            },
            {
                Keyboard.Key.F9,
                Button.F9
            },
            {
                Keyboard.Key.F10,
                Button.F10
            },
            {
                Keyboard.Key.F11,
                Button.F11
            },
            {
                Keyboard.Key.F12,
                Button.F12
            },
            {
                Keyboard.Key.Num0,
                Button.Zero
            },
            {
                Keyboard.Key.Num1,
                Button.One
            },
            {
                Keyboard.Key.Num2,
                Button.Two
            },
            {
                Keyboard.Key.Num3,
                Button.Three
            },
            {
                Keyboard.Key.Num4,
                Button.Four
            },
            {
                Keyboard.Key.Num5,
                Button.Five
            },
            {
                Keyboard.Key.Num6,
                Button.Six
            },
            {
                Keyboard.Key.Num7,
                Button.Seven
            },
            {
                Keyboard.Key.Num8,
                Button.Eight
            },
            {
                Keyboard.Key.Num9,
                Button.Nine
            }
        };

        private Dictionary<uint, Button> joyMap = new Dictionary<uint, Button>
        {
            {
                0U,
                Button.A
            },
            {
                1U,
                Button.B
            },
            {
                2U,
                Button.X
            },
            {
                3U,
                Button.Y
            },
            {
                4U,
                Button.L
            },
            {
                5U,
                Button.R
            },
            {
                6U,
                Button.Select
            },
            {
                7U,
                Button.Start
            },
            {
                8U,
                Button.Tilde
            }
        };

        private Dictionary<Button, bool> currentState;

        private float xAxis;

        private float yAxis;

        private float xKeyAxis;

        private float yKeyAxis;

        private bool axisZero;

        private bool axisZeroLast;

        private bool enabled;

        private bool leftPress;

        private bool rightPress;

        private bool upPress;

        private bool downPress;

        public delegate void ButtonPressedHandler(InputManager sender, Button b);

        public delegate void ButtonReleasedHandler(InputManager sender, Button b);

        public delegate void AxisPressedHandler(InputManager sender, Vector2f axis);

        public delegate void AxisReleasedHandler(InputManager sender, Vector2f axis);
    }
}

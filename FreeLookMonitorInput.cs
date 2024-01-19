using LethalCompanyInputUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;

namespace FreeLookMonitor {
    internal class FreeLookMonitorInput : LcInputActions {

        [InputAction("<Keyboard>/upArrow", Name = "Move forward")]
        public InputAction MoveForward { get; set;}

        [InputAction("<Keyboard>/downArrow", Name = "Move backward")]
        public InputAction MoveBackward { get; set;}

        [InputAction("<Keyboard>/leftArrow", Name = "Move left")]
        public InputAction MoveLeft { get; set; }

        [InputAction("<Keyboard>/rightArrow", Name = "Move right")]
        public InputAction MoveRight { get; set;}

        [InputAction("<Keyboard>/pageUp", Name = "Move up")]
        public InputAction MoveUp { get; set; }

        [InputAction("<Keyboard>/pageDown", Name = "Move down")]
        public InputAction MoveDown { get; set; }

        [InputAction("<Keyboard>/insert", Name = "Reset position")]
        public InputAction ResetPosition { get; set; }

        [InputAction("<Keyboard>/plus", Name = "Zoom in")]
        public InputAction ZoomIn { get; set; }

        [InputAction("<Keyboard>/minus", Name = "Zoom out")]
        public InputAction ZoomOut { get; set; }



    }

    
}

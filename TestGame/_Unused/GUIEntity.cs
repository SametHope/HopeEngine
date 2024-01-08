//using Raylib_CsLo;

//namespace HopeEngine;

//public class GUIEntity : Entity
//{
//    public Rectangle Rect;
//    public event Action? OnMouseEnter, OnMouseExit, OnMouseStay, OnClick;

//    protected bool _isMouseOver;
//    protected bool _wasMouseOver;

//    public override void Update(float deltaTime)
//    {
//        _wasMouseOver = _isMouseOver;
//        _isMouseOver = Raylib.CheckCollisionPointRec(Engine.MouseScreenPosition, Rect);

//        if (_isMouseOver && Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
//        {
//            OnClick?.Invoke();
//        }
//        else if (!_wasMouseOver && _isMouseOver)
//        {
//            OnMouseEnter?.Invoke();
//        }
//        else if (_wasMouseOver && !_isMouseOver)
//        {
//            OnMouseExit?.Invoke();
//        }
//        else if (_wasMouseOver && _isMouseOver)
//        {
//            OnMouseStay?.Invoke();
//        }
//    }
//}

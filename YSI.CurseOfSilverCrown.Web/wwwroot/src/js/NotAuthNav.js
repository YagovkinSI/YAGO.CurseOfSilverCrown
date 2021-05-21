import { NavLink } from 'react-router-dom';

export default function NotAuthNav() {
  return (
    <ul className="navbar-nav">
      <li className="nav-item">
        <NavLink className="nav-link" to="/registration">Регистрация</NavLink>
      </li>
      <li className="nav-item">
        <NavLink className="nav-link" to="/login">Войти</NavLink>
      </li>
    </ul>
  );
}

export default function AuthNav(props) {
  return (
    <ul className="navbar-nav">
      <li className="nav-item">
        <a className="nav-link text-dark" > {/*asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage"*/}
          Здравствуйте, {props.userName}!
        </a>
      </li>
      <li className="nav-item">
        <form className="form-inline">
          <button className="nav-link btn btn-link text-dark" onClick={props.onLogout}>Выйти</button>
        </form>
      </li>
    </ul>
  );
}

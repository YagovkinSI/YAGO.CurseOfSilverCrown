export default function LoginPartial(props) {
  console.log(props)
  if (props.currentUser.isSignedIn) {
    return (
      <ul className="navbar-nav">
        <li className="nav-item">
          <a className="nav-link text-dark" > {/*asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage"*/}
            Здравствуйте, {props.currentUser.userName}!
          </a>
        </li>
        <li className="nav-item">
          <form className="form-inline">
            <button className="nav-link btn btn-link text-dark" onClick={() => props.onUserLogout}>Выйти</button>
          </form>
        </li>
      </ul >
    );
  } else {
    return (
      <ul className="navbar-nav">
        {/*<li className="nav-item">*/}
        {/*    <a className="nav-link text-dark" asp-area="Identity" asp-page="/Account/Register">Регистрация</a>*/}
        {/*</li>*/}
        <li className="nav-item">
          <a className="nav-link text-dark" onClick={() => props.changeActivePage('login')}>Вход</a>
        </li>
      </ul >
    );
  }        

}
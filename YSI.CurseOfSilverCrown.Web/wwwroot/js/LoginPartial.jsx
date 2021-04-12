export default class LoginPartial extends React.Component {   
  constructor(props) {
      super(props);
      this.onLoginClick = this.onLoginClick.bind(this);
      this.onLogoutClick = this.onLogoutClick.bind(this);
  }
  onLoginClick() {
      this.props.changeMainPage("login");
  }
  onLogoutClick() {
      this.props.onUserLogout();
  }
  render() {
      if (this.props.currentUser.isSignedIn) {
          return <ul className="navbar-nav">
              <li className="nav-item">
                  <a className="nav-link text-dark" > {/*asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage"*/}
                      Здравствуйте, {this.props.currentUser.userName}!
                  </a>
              </li>
              <li className="nav-item">
                  <form className="form-inline">
                      <button className="nav-link btn btn-link text-dark" onClick={this.onLogoutClick}>Выйти</button>
                  </form>
              </li>
          </ul >
      } else {
          return <ul className="navbar-nav">                
              {/*<li className="nav-item">*/}
              {/*    <a className="nav-link text-dark" asp-area="Identity" asp-page="/Account/Register">Регистрация</a>*/}
              {/*</li>*/}
              <li className="nav-item">
                  <a className="nav-link text-dark" onClick={this.onLoginClick}>Вход</a>
              </li>
          </ul >;
      }        
  }
}
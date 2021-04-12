export default class HomeIndex extends React.Component {
  constructor(props) {
      super(props);
      this.state = {
          turnName: "",
          lastEventStories: [[]]
      };
  }
  loadData() {
      var xhr = new XMLHttpRequest();
      xhr.open("get", "api/Home/Index", true);
      xhr.onload = function () {
          let data = JSON.parse(xhr.responseText);
          this.setState({
              turnName: data.turn,
              lastEventStories: data.lastEventStories
          });
      }.bind(this);
      xhr.send();
  }
  componentDidMount() {
      this.loadData();
  }
  render() {
      return <>
          <div className="text-center">
              <h1 className="display-4">Добро пожаловать</h1>

              <p>Узнай о том как <a href="https://vk.com/club189975977">разрабатывается игра</a>.</p>

              <h3>На дворе {this.state.turnName}</h3>

              {/*@{}*/}
              {/*        var user = UserManager.GetUserAsync(User).Result;*/}
              {/*        var isAdmin = user != null*/}
              {/*        ?UserManager.IsInRoleAsync(user, "Admin").Result*/}
              {/*        : false;*/}
              {/*        if (isAdmin)*/}
              {/*        {<a className="nav-link text-dark" asp-area="" asp-controller="Admin" asp-action="NextTurn">Закончить ход</a>}*/}
              {/*    }*/}
          </div>
          <div>
              <h4>
                  Важнейшие события прошлого
              </h4>
              {/*{*/}
              {/*    this.state.lastEventStories.map(function (eventStory) {*/}
              {/*        return <>*/}
              {/*            {*/}
              {/*                eventStory.map(function (line) {*/}
              {/*                    return*/}
              {/*                    <p style="margin-bottom: 0;">{line}</p>*/}
              {/*                })*/}
              {/*            }*/}
              {/*            <p style="margin-bottom: 0;" > ____________________</p>*/}
              {/*        </>*/}
              {/*    })*/}
              {/*}*/}
          </div>
      </>
  }
}

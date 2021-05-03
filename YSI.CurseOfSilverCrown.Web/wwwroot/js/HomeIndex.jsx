import TurnEvents from "./TurnEvents.jsx";

export default function HomeIndex () {
  const [turnName, setTurnName] = React.useState('');
  const [lastEventStories, setLastEventStories] = React.useState([[]]);

  React.useEffect(() => {
    var xhr = new XMLHttpRequest();
    xhr.open("get", "api/Home/Index", true);
    xhr.onload = () => {
      let data = JSON.parse(xhr.responseText);
      setTurnName(data.turn);
      setLastEventStories(data.lastEventStories);
    };
    xhr.send();
  }, []);

  const lastEvents = lastEventStories.map((eventStory, index) => {
    return (
      <div key={eventStory[0] + index}>
        <TurnEvents events={eventStory}/>
        {index === lastEventStories.length - 1 ? null : <hr/>}
      </div>
    );
  });

  return (
    <div className="container">
      <main className="pb-3" role="main">
        <div className="text-center">
          <h1 className="display-4">Добро пожаловать</h1>
          <p>Узнай о том как <a href="https://vk.com/club189975977">разрабатывается игра</a>.</p>
          <h3>На дворе {turnName}</h3>
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
          {lastEvents}
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
      </main>
    </div>
  );
}

import styled from 'styled-components';
import logo from '/favicon.png'

const StyledImg = styled.img`
  width: 60%;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;

  &:hover {
    filter: drop-shadow(0 0 2em #b2b448aa);
  }
`;

function App() {
  return (
    <>
      <div>
        <a href="/">
          <StyledImg src={logo} className="logo" alt="Yago World logo" />
        </a>
      </div>
      <h1>Yago World</h1>
    </>
  )
}

export default App
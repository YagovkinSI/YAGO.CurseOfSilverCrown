import styled from 'styled-components';
import logo from '/favicon.png'
import YagoCard from './shared/YagoCard';

const StyledImg = styled.img`
  max-width: 60%;
  max-heigth: 60%;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;

  &:hover {
    filter: drop-shadow(0 0 2em var(--color-bright));
  }
`;

function HomePage() {

  return (
    <YagoCard title={'Yago World'} >
      <div>
        <a href="/">
          <StyledImg src={logo} className="logo" alt="Yago World logo" />
        </a>
      </div>
    </YagoCard>
  )
}

export default HomePage
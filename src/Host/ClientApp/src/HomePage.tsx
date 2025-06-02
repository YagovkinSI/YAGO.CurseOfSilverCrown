import styled from 'styled-components';
import logo from '/favicon.png'
import { Typography } from '@mui/material';
import YagoCard from './shared/YagoCard';
import ButtonWithLink from './shared/ButtonWithLink';

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
    <YagoCard image={undefined}  >
        <div>
          <a href="/">
            <StyledImg src={logo} className="logo" alt="Yago World logo" />
          </a>
        </div>
        <Typography variant="h1" gutterBottom>Yago World</Typography>
        <ButtonWithLink to={'/app/map/'} text={'Закрыть'} />
    </YagoCard>
  )
}

export default HomePage
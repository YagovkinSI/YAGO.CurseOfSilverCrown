import styled from 'styled-components';
import logo from '/favicon.png'
import { Card, CardContent, Typography } from '@mui/material';

const StyledImg = styled.img`
  width: 60%;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;

  &:hover {
    filter: drop-shadow(0 0 2em var(--color-bright));
  }
`;

function HomePage() {
  return (
    <Card
      style={{
        backgroundColor: 'rgba(255, 255, 255, 0.9)',
        boxShadow: '4px 4px 10px rgba(0, 0, 0, 0.7)'
      }}
    >
      <CardContent>
        <div>
          <a href="/">
            <StyledImg src={logo} className="logo" alt="Yago World logo" />
          </a>
        </div>
        <Typography variant="h1" gutterBottom>Yago World</Typography>
      </CardContent>
    </Card>
  )
}

export default HomePage
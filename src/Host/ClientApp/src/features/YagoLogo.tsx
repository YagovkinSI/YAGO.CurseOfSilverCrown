import styled from 'styled-components';
import logo from '/favicon.png'
import { useNavigate } from 'react-router-dom';

const StyledImg = styled.img`
  max-width: 60%;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;

  &:hover {
    filter: drop-shadow(0 0 2em var(--color-bright));
  }
`;

const YagoLogo: React.FC = () => {
    const navigate = useNavigate();

    return (
        <StyledImg
            src={logo}
            className="logo"
            alt="Yago World logo"
            onClick={() => navigate('/app/home')}
        />
    )
}

export default YagoLogo
import { type MockHandler } from 'vite-plugin-mock-server'

export const WAARDELIJST = {
  ORGANISATIE: 'Organisatie',
  THEMA: 'Thema',
  INFORMATIECATEGORIE: 'Informatiecategorie'
} as const

export type WaardeLijsten = keyof typeof WAARDELIJST

export type WaardeLijstItem = {
  id: number
  type: WaardeLijsten
  name: string
  checked?: boolean
}

const mocks: MockHandler[] = [
  {
    pattern: '/api/waardelijsten',
    handle: (_req, res) => {
      const data: WaardeLijstItem[] = [
        {
          id: 1,
          type: 'ORGANISATIE',
          name: 'Organisatie 1'
        },
        {
          id: 2,
          type: 'ORGANISATIE',
          name: 'Organisatie 2'
        },
        {
          id: 3,
          type: 'ORGANISATIE',
          name: 'Organisatie 3'
        },
        {
          id: 4,
          type: 'THEMA',
          name: 'europese zaken'
        },
        {
          id: 5,
          type: 'THEMA',
          name: 'afval'
        },
        {
          id: 6,
          type: 'THEMA',
          name: 'arbeidsomstandigheden'
        },
        {
          id: 7,
          type: 'THEMA',
          name: 'arbeidsongeschikheid'
        },
        {
          id: 8,
          type: 'INFORMATIECATEGORIE',
          name: 'wet of algemeen bindend voorschrift'
        }
      ]

      res.setHeader('Content-Type', 'application/json')
      res.end(JSON.stringify(data))
    }
  }
  //   {
  //     pattern: '/api/test/json',
  //     method: 'POST',
  //     handle: (req, res) => {
  //       res.setHeader('Content-Type', 'application/json')

  //       req.on('data', (bodyString: string) => {
  //         const body: object = JSON.parse(bodyString)
  //         res.end(JSON.stringify(body))
  //       })
  //     }
  //   }
]

export default mocks
